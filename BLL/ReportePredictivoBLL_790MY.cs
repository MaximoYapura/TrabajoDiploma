using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    // Reporte predictivo (análisis de canasta): busca qué productos se piden juntos.
    // Toma las facturas pagadas de los últimos 90 días; cada factura es una canasta con
    // los productos distintos que tuvo. Para cada par calcula:
    //   soporte = cuentas con A y B / total de cuentas
    //   confianza = cuentas con A y B / cuentas con A
    // y se queda con la dirección (A->B o B->A) de mayor confianza.
    // Está hecho a mano, sin librerías: con una carta de decenas de productos es rápido.
    // Permiso: GestionReportePredictivo.
    public class ReportePredictivoBLL_790MY
    {
        public const int DiasAnalisis = 90;

        // Con menos cuentas que esto no se calculan reglas porque no serían confiables.
        public const int MinimoCuentasReales = 10;

        // Soporte mínimo (%) para considerar frecuente una combinación.
        public const decimal SoporteMinimo = 3m;

        // Una combinación debe aparecer al menos en esta cantidad de cuentas.
        private const int MinimoCuentasPar = 2;

        // Lift mínimo. Descarta pares que aparecen juntos solo porque B se pide mucho.
        public const decimal LiftMinimo = 1.2m;

        // Confianza mínima (%) de las reglas que pueden originar una sugerencia.
        public const decimal ConfianzaMinimaSugerencias = 40m;

        private readonly IFacturaRepository_790MY _facturaRepository;

        private List<CanastaFactura_790MY> _canastas;
        private List<ReglaAsociacion_790MY> _reglas;
        private ResumenCanasta_790MY _resumen;

        public ReportePredictivoBLL_790MY(IFacturaRepository_790MY facturaRepository)
        {
            _facturaRepository = facturaRepository ?? throw new ArgumentNullException(nameof(facturaRepository));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Consultas del reporte
        // ═══════════════════════════════════════════════════════════════════

        // Descarta lo calculado para que la próxima consulta vuelva a leer la base.
        public void Actualizar()
        {
            _canastas = null;
            _reglas = null;
            _resumen = null;
        }

        // Cuántas cuentas se analizaron y desde qué fecha.
        public ResumenCanasta_790MY ObtenerResumen()
        {
            ValidarAcceso();
            Analizar();
            return _resumen;
        }

        // Reglas con confianza mayor o igual a la indicada (en %), ordenadas y numeradas.
        public List<ReglaAsociacion_790MY> ObtenerReglasAsociacion(decimal confianzaMinima,
                                                                   OrdenReglas_790MY orden = OrdenReglas_790MY.PorConfianza)
        {
            ValidarAcceso();
            if (confianzaMinima < 0 || confianzaMinima > 100)
                throw new ArgumentOutOfRangeException(nameof(confianzaMinima));

            Analizar();
            IEnumerable<ReglaAsociacion_790MY> filtradas = _reglas.Where(r => r.Confianza >= confianzaMinima);
            filtradas = orden == OrdenReglas_790MY.PorSoporte
                ? filtradas.OrderByDescending(r => r.Soporte).ThenByDescending(r => r.Confianza)
                : filtradas.OrderByDescending(r => r.Confianza).ThenByDescending(r => r.Soporte);

            int numero = 1;
            return filtradas.Select(r => Copiar(r, numero++)).ToList();
        }

        // Hasta tres sugerencias con las reglas más fuertes:
        // 1. un combo con la regla de mayor confianza;
        // 2. una promoción el día con menos cuentas, con otra regla que no repita productos;
        // 3. un aviso de venta cruzada para el camarero con la regla de más soporte que queda.
        public List<SugerenciaInteligente_790MY> ObtenerSugerenciasInteligentes()
        {
            var sugerencias = new List<SugerenciaInteligente_790MY>();
            List<ReglaAsociacion_790MY> fuertes = ObtenerReglasAsociacion(ConfianzaMinimaSugerencias);
            if (fuertes.Count == 0) return sugerencias;

            ReglaAsociacion_790MY combo = fuertes[0];
            sugerencias.Add(new SugerenciaInteligente_790MY { Tipo = TipoSugerencia_790MY.CrearCombo, Regla = combo });

            ReglaAsociacion_790MY promo = fuertes.Skip(1).FirstOrDefault(r => !CompartenProducto(r, combo))
                                       ?? fuertes.Skip(1).FirstOrDefault();
            if (promo != null)
                sugerencias.Add(new SugerenciaInteligente_790MY
                {
                    Tipo = TipoSugerencia_790MY.PromocionDiaBajaDemanda,
                    Regla = promo,
                    DiaMenorDemanda = DiaDeMenorDemanda()
                });

            ReglaAsociacion_790MY cruzada = fuertes
                .Where(r => r != combo && r != promo)
                .OrderByDescending(r => r.Soporte).ThenByDescending(r => r.Confianza)
                .FirstOrDefault();
            if (cruzada != null)
                sugerencias.Add(new SugerenciaInteligente_790MY { Tipo = TipoSugerencia_790MY.VentaCruzadaCamarero, Regla = cruzada });

            return sugerencias;
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Motor de reglas de asociación
        // ═══════════════════════════════════════════════════════════════════

        // Lee las canastas y calcula las reglas una sola vez por instancia.
        private void Analizar()
        {
            if (_reglas != null) return;

            DateTime desde = DateTime.Today.AddDays(-DiasAnalisis);
            List<CanastaFactura_790MY> canastas = _facturaRepository.ObtenerCanastasPagadas(desde)
                .Where(c => c.Productos.Count > 0)
                .ToList();

            bool suficientes = canastas.Count >= MinimoCuentasReales;

            _canastas = canastas;
            _reglas = suficientes ? CalcularReglas(canastas) : new List<ReglaAsociacion_790MY>();
            _resumen = new ResumenCanasta_790MY
            {
                CuentasAnalizadas = canastas.Count,
                DiasAnalizados = DiasAnalisis,
                Desde = desde,
                DatosSuficientes = suficientes
            };
        }

        // Cuenta en cuántas canastas aparece cada producto y cada par de productos, y arma
        // las reglas de los pares que pasan los mínimos.
        internal static List<ReglaAsociacion_790MY> CalcularReglas(IList<CanastaFactura_790MY> canastas)
        {
            var reglas = new List<ReglaAsociacion_790MY>();
            int total = canastas.Count;
            if (total == 0) return reglas;

            var nombres = new Dictionary<int, string>();
            var cuentasProducto = new Dictionary<int, int>();
            var cuentasPar = new Dictionary<(int, int), int>();

            foreach (CanastaFactura_790MY canasta in canastas)
            {
                int[] ids = canasta.Productos.Keys.OrderBy(id => id).ToArray();
                foreach (int id in ids)
                {
                    nombres[id] = canasta.Productos[id];
                    cuentasProducto[id] = cuentasProducto.TryGetValue(id, out int n) ? n + 1 : 1;
                }
                for (int i = 0; i < ids.Length; i++)
                    for (int j = i + 1; j < ids.Length; j++)
                    {
                        var par = (ids[i], ids[j]);
                        cuentasPar[par] = cuentasPar.TryGetValue(par, out int n) ? n + 1 : 1;
                    }
            }

            foreach (KeyValuePair<(int, int), int> par in cuentasPar)
            {
                int conAyB = par.Value;
                decimal soporte = Porcentaje(conAyB, total);
                if (conAyB < MinimoCuentasPar || soporte < SoporteMinimo) continue;

                int a = par.Key.Item1, b = par.Key.Item2;
                // Se informa la dirección más fuerte: "si pide X -> también pide Y".
                decimal confAB = Porcentaje(conAyB, cuentasProducto[a]);
                decimal confBA = Porcentaje(conAyB, cuentasProducto[b]);
                if (confBA > confAB || (confBA == confAB && string.CompareOrdinal(nombres[b], nombres[a]) < 0))
                {
                    int tmp = a; a = b; b = tmp;
                }

                decimal confianza = Porcentaje(conAyB, cuentasProducto[a]);
                // Lift = P(A y B) / (P(A)·P(B)), calculado con los conteos sin redondear.
                decimal lift = Math.Round(conAyB * (decimal)total / ((decimal)cuentasProducto[a] * cuentasProducto[b]), 2);
                if (lift < LiftMinimo) continue;

                reglas.Add(new ReglaAsociacion_790MY
                {
                    ProductoAId = a,
                    ProductoA = nombres[a],
                    ProductoBId = b,
                    ProductoB = nombres[b],
                    CuentasConAyB = conAyB,
                    CuentasConA = cuentasProducto[a],
                    Soporte = soporte,
                    Confianza = confianza,
                    Lift = lift
                });
            }
            return reglas;
        }

        // Día de la semana con menos cuentas. Se promedia por cuántas veces aparece ese día
        // en el período, para no perjudicar a un día que aparece una vez menos.
        private DayOfWeek DiaDeMenorDemanda()
        {
            Analizar();
            DateTime desde = _resumen.Desde;
            var ocurrencias = new Dictionary<DayOfWeek, int>();
            for (DateTime d = desde; d < DateTime.Today.AddDays(1); d = d.AddDays(1))
                ocurrencias[d.DayOfWeek] = ocurrencias.TryGetValue(d.DayOfWeek, out int n) ? n + 1 : 1;

            var cuentas = _canastas.GroupBy(c => c.FechaEmision.DayOfWeek).ToDictionary(g => g.Key, g => g.Count());
            return Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
                .OrderBy(dia => (cuentas.TryGetValue(dia, out int c) ? c : 0) / (double)Math.Max(1, ocurrencias.TryGetValue(dia, out int o) ? o : 1))
                .First();
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private static decimal Porcentaje(int parte, int total)
            => total == 0 ? 0 : Math.Round(parte * 100m / total, 1, MidpointRounding.AwayFromZero);

        private static bool CompartenProducto(ReglaAsociacion_790MY x, ReglaAsociacion_790MY y)
            => x.ProductoAId == y.ProductoAId || x.ProductoAId == y.ProductoBId
            || x.ProductoBId == y.ProductoAId || x.ProductoBId == y.ProductoBId;

        private static ReglaAsociacion_790MY Copiar(ReglaAsociacion_790MY r, int numero) => new ReglaAsociacion_790MY
        {
            Numero = numero,
            ProductoAId = r.ProductoAId,
            ProductoA = r.ProductoA,
            ProductoBId = r.ProductoBId,
            ProductoB = r.ProductoB,
            CuentasConAyB = r.CuentasConAyB,
            CuentasConA = r.CuentasConA,
            Soporte = r.Soporte,
            Confianza = r.Confianza,
            Lift = r.Lift
        };

        // Todas las consultas del reporte piden el mismo permiso.
        private static void ValidarAcceso()
            => SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionReportePredictivo);
    }
}
