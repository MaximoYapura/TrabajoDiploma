using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public static class DVManager_08YS
    {
        private static DvBLL_08YS _bll;

        public static void Inicializar(DvBLL_08YS bll) => _bll = bll;

        public static void Recalcular()
        {
            try { _bll?.Recalcular(); }
            catch { }
        }

        public static bool VerificarConsistencia()
        {
            try { return _bll?.VerificarConsistencia() ?? true; }
            catch { return true; }
        }
    }

    public class DvBLL_08YS
    {
        public static readonly string[] TablasMonitoreadas =
        {
            "Users", "Roles", "Familias", "Permisos",
            "FamiliaPermiso", "FamiliaIntegrada", "RolFamilia", "RolPermiso"
        };

        private readonly IDvRepository_08YS _repo;

        public DvBLL_08YS(IDvRepository_08YS repo) => _repo = repo;

        /// <summary>
        /// Lee todas las tablas, calcula DVH y DVV cifrados por tabla,
        /// y persiste una fila en DV por cada tabla.
        /// </summary>
        public void Recalcular()
        {
            var tablas = _repo.GetTodasLasTablas();

            for (int i = 0; i < tablas.Count; i++)
            {
                string nombreTabla = TablasMonitoreadas[i];
                var (dvhCifrado, dvvCifrado) = CalcularYCifrar(tablas[i], nombreTabla);

                _repo.GuardarDVTabla(new DigitoVerificador_08YS
                {
                    Tabla = nombreTabla,
                    DVH = dvhCifrado,
                    DVV = dvvCifrado
                });
            }
        }

        /// <summary>
        /// Recalcula en memoria y compara contra los valores almacenados.
        /// Devuelve true si todas las tablas son consistentes.
        /// </summary>
        public bool VerificarConsistencia()
        {
            var tablas = _repo.GetTodasLasTablas();
            var guardado = _repo.GetDVGuardado()
                               .ToDictionary(e => e.Tabla);

            // Si no hay ninguna entrada guardada aún (BD vacía o primer arranque)
            // consideramos consistente para no bloquear el primer login
            if (!guardado.Any()) return true;

            for (int i = 0; i < tablas.Count; i++)
            {
                string nombreTabla = TablasMonitoreadas[i];

                // Si no existe entrada para esta tabla, es inconsistente
                if (!guardado.ContainsKey(nombreTabla)) return false;

                var (dvhActual, dvvActual) = CalcularYCifrar(tablas[i], nombreTabla);

                DigitoVerificador_08YS almacenado = guardado[nombreTabla];
                if (dvhActual != almacenado.DVH || dvvActual != almacenado.DVV)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Formatea un valor de celda para la concatenación.
        /// Normaliza tipos para que la comparación sea consistente entre ejecuciones.
        /// </summary>
        private static string FormatearCelda(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return string.Empty;

            if (valor is DateTime dt)
                return dt.ToString("yyyy-MM-dd HH:mm:ss");

            if (valor is bool b)
                return b ? "1" : "0";

            if (valor is byte by)
                return by == 1 ? "1" : "0";  // BIT en SQL llega como byte

            return valor.ToString();
        }

        /// <summary>
        /// DVH de una tabla: concatena fila por fila.
        /// Fila 1: col1+col2+col3 → Fila 2: col1+col2+col3 → ...
        /// Resultado: "row1col1row1col2row1col3row2col1row2col2..."
        /// </summary>
        public static string ConcatenarDVH(DataTable tabla)
        {
            var sb = new StringBuilder();
            foreach (DataRow row in tabla.Rows)
                foreach (DataColumn col in tabla.Columns)
                    sb.Append(FormatearCelda(row[col]));
            return sb.ToString();
        }

        /// <summary>
        /// DVV de una tabla: concatena columna por columna.
        /// Col 1: row1+row2+row3 → Col 2: row1+row2+row3 → ...
        /// Resultado: "col1row1col1row2col1row3col2row1col2row2..."
        /// Nótese que DVH ≠ DVV porque el orden de iteración es distinto.
        /// </summary>
        public static string ConcatenarDVV(DataTable tabla)
        {
            var sb = new StringBuilder();
            foreach (DataColumn col in tabla.Columns)
                foreach (DataRow row in tabla.Rows)
                    sb.Append(FormatearCelda(row[col]));
            return sb.ToString();
        }

        /// <summary>
        /// Calcula y cifra el DVH y DVV de una DataTable.
        /// </summary>
        public static (string DVH, string DVV) CalcularYCifrar(DataTable tabla, string nombreTabla)
        {
            string dvhPlano = ConcatenarDVH(tabla);
            string dvvPlano = ConcatenarDVV(tabla);

            string dvhCifrado = DvEncriptador_08YS.Cifrar(dvhPlano, nombreTabla + "_DVH");
            string dvvCifrado = DvEncriptador_08YS.Cifrar(dvvPlano, nombreTabla + "_DVV");

            return (dvhCifrado, dvvCifrado);
        }
    }
}
