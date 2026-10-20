using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    public enum OrdenReglas_790MY
    {
        PorConfianza,
        PorSoporte
    }

    /// <summary>"Si pide A -> también pide B", con sus métricas expresadas en porcentaje (0-100).</summary>
    public class ReglaAsociacion_790MY
    {
        public int Numero { get; set; }
        public int ProductoAId { get; set; }
        public string ProductoA { get; set; }
        public int ProductoBId { get; set; }
        public string ProductoB { get; set; }

        /// <summary>Cuentas que incluyen A y B.</summary>
        public int CuentasConAyB { get; set; }

        /// <summary>Cuentas que incluyen A.</summary>
        public int CuentasConA { get; set; }

        /// <summary>Soporte % = cuentas con A y B / total de cuentas.</summary>
        public decimal Soporte { get; set; }

        /// <summary>Confianza % = cuentas con A y B / cuentas con A.</summary>
        public decimal Confianza { get; set; }

        /// <summary>Lift = Confianza / P(B). Mayor a 1 indica asociación real (no azar).</summary>
        public decimal Lift { get; set; }
    }

    public enum TipoSugerencia_790MY
    {
        /// <summary>Regla de mayor confianza: armar un combo con ambos productos.</summary>
        CrearCombo,
        /// <summary>Otra regla fuerte, promocionada el día de la semana con menos cuentas.</summary>
        PromocionDiaBajaDemanda,
        /// <summary>Regla de mayor soporte: aviso de venta cruzada en la comanda del camarero.</summary>
        VentaCruzadaCamarero
    }

    /// <summary>
    /// Recomendación de negocio derivada de las reglas. La BLL entrega los datos y la GUI
    /// arma el texto en el idioma activo (claves RI_*).
    /// </summary>
    public class SugerenciaInteligente_790MY
    {
        public TipoSugerencia_790MY Tipo { get; set; }
        public ReglaAsociacion_790MY Regla { get; set; }

        /// <summary>Solo para PromocionDiaBajaDemanda.</summary>
        public DayOfWeek? DiaMenorDemanda { get; set; }
    }

    public class ResumenCanasta_790MY
    {
        public int CuentasAnalizadas { get; set; }
        public int DiasAnalizados { get; set; }
        public DateTime Desde { get; set; }

        /// <summary>
        /// false si la BD todavía no tiene suficientes cuentas cobradas para calcular
        /// asociaciones: no se generan reglas ni sugerencias y la GUI lo informa.
        /// </summary>
        public bool DatosSuficientes { get; set; }
    }

    /// <summary>
    /// Reporte inteligente: Análisis Predictivo de Canasta (Market Basket Analysis).
    ///
    /// Toma las facturas Pagadas de los últimos 90 días; cada factura es una "canasta"
    /// con los productos distintos que se pidieron en la misma cuenta. Para cada par de
    /// productos calcula:
    ///   Soporte %   = (cuentas con A y B) / (total de cuentas)
    ///   Confianza % = (cuentas con A y B) / (cuentas con A)
    /// y conserva la dirección (A -> B o B -> A) de mayor confianza. Implementación
    /// propia, sin librerías externas: con la carta de un restaurante (decenas de
    /// productos) el conteo de pares por canasta es inmediato.
    ///
    /// Patente: GestionReportePredictivo.
    /// </summary>
    public class ReportePredictivoBLL_790MY
    {
        public const int DiasAnalisis = 90;

        /// <summary>
        /// Cuentas cobradas mínimas para calcular reglas. Por debajo de este umbral el
        /// soporte de cualquier par sería anecdótico. Siempre se analizan datos reales
        /// (Facturas pagadas + ItemsFactura), con o sin ModoDemostracion.
        /// </summary>
        public const int MinimoCuentasReales = 10;

        /// <summary>Soporte mínimo (%) para considerar frecuente una combinación.</summary>
        public const decimal SoporteMinimo = 3m;

        /// <summary>Una combinación debe aparecer al menos en esta cantidad de cuentas.</summary>
        private const int MinimoCuentasPar = 2;

        /// <summary>
        /// Lift mínimo: descarta pares que coinciden solo porque B es muy pedido
        /// (lift ≈ 1 = independencia), aunque su confianza sea alta.
        /// </summary>
        public const decimal LiftMinimo = 1.2m;

        /// <summary>Confianza mínima (%) de las reglas que pueden originar una sugerencia.</summary>
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
        //  API del reporte
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Vuelve a leer las facturas de la BD en la próxima consulta.</summary>
        public void Actualizar()
        {
            _canastas = null;
            _reglas = null;
            _resumen = null;
        }

        public ResumenCanasta_790MY ObtenerResumen()
        {
            ValidarAcceso();
            Analizar();
            return _resumen;
        }

        /// <summary>
        /// Reglas con confianza mayor o igual a la indicada (en %), ordenadas por confianza
        /// o por soporte y numeradas según ese orden.
        /// </summary>
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

        /// <summary>
        /// Hasta tres recomendaciones concretas a partir de las reglas más fuertes:
        ///  1. Combo con la regla de mayor confianza.
        ///  2. Promoción el día de menor demanda con la siguiente regla fuerte que no
        ///     comparta productos con el combo.
        ///  3. Aviso de venta cruzada al camarero con la regla de mayor soporte restante.
        /// </summary>
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

        /// <summary>
        /// Cuenta, en una sola pasada, en cuántas canastas aparece cada producto y cada par
        /// de productos; luego arma las reglas de los pares frecuentes.
        /// </summary>
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

        /// <summary>
        /// Día de la semana con menos cuentas cobradas en el período (promedio por
        /// ocurrencia de ese día, para no penalizar días que aparecen una vez menos).
        /// </summary>
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

        /// <summary>Patente dedicada del reporte predictivo.</summary>
        private static void ValidarAcceso()
            => SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionReportePredictivo);
    }
}
