using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public abstract class AccesoBLL_08YS
    {
        protected readonly IPermisoRepository_08YS _permisoRepo;

        protected AccesoBLL_08YS(IPermisoRepository_08YS permisoRepo)
        {
            _permisoRepo = permisoRepo;
        }

        /// <summary>
        /// Evalúa si el candidato puede agregarse a la lista actual.
        /// <para>— Valido: sin solapamiento, proceder.</para>
        /// <para>— SugerenciaReemplazo: solapamiento total con items existentes que el candidato
        ///         engloba completamente → proponer reemplazo al usuario.</para>
        /// <para>— ConflictoIrresoluble: solapamiento con items que el candidato no engloba
        ///         completamente → error, no se puede resolver.</para>
        /// </summary>
        public ResultadoEvaluacion_08YS EvaluarAgregarComponente(
            HashSet<AccessComponent_08YS> listaActual,
            AccessComponent_08YS candidato)
        {
            var permsCandidato = candidato.GetPermisos()
                .Select(p => p.PermisoID).ToHashSet();

            var contemplados = listaActual
                .Where(item => item.GetPermisos()                       
                    .All(p => permsCandidato.Contains(p.PermisoID)))        // Solo los componentes cuyos permisos estan 
                .ToList();                                                  // completamente contenidos en el candidato

            var permsEnConflicto = listaActual
                .Except(contemplados, AccessComponentComparer_08YS.Instance) // Solo los que no se van a reemplazar
                .SelectMany(i => i.GetPermisos())                            // Todos los permisos de esos componentes restantes
                .Select(p => p.PermisoID)                                
                .Where(id => permsCandidato.Contains(id))                    // Solo los permisos presentes en el candidato que generan conflicto
                .ToList();

            if (permsEnConflicto.Any())
            {
                var nombres = _permisoRepo.GetAll()
                    .Where(p => permsEnConflicto.Contains(p.PermisoID))
                    .Select(p => p.Nombre);
                string plantillaConflicto = TraductorManager_08YS.Instance.GetTexto("bll_composite_conflicto");
                return new ResultadoEvaluacion_08YS
                {
                    Resultado = ResultadoEvaluacion_08YS.Tipo.ConflictoIrresoluble,
                    Mensaje = string.Format(plantillaConflicto, candidato.Nombre, string.Join(", ", nombres))
                };
            }

            if (contemplados.Any())
            {
                var nombresReemplazar = contemplados.Select(i => $"'{i.Nombre}'");
                string plantillaSugerencia = TraductorManager_08YS.Instance.GetTexto("bll_composite_sugerencia");
                return new ResultadoEvaluacion_08YS
                {
                    Resultado = ResultadoEvaluacion_08YS.Tipo.SugerenciaReemplazo,
                    ComponentesAReemplazar = contemplados,
                    Mensaje = string.Format(plantillaSugerencia, candidato.Nombre, string.Join(", ", nombresReemplazar))
                };
            }

            return new ResultadoEvaluacion_08YS { Resultado = ResultadoEvaluacion_08YS.Tipo.Valido };
        }

        protected void ValidarDatosEntrada(string nombre, HashSet<AccessComponent_08YS> componentes)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new InvalidOperationException(TraductorManager_08YS.Instance.GetTexto("bll_val_nombre_vacio"));

            if (componentes == null || !componentes.Any())
                throw new InvalidOperationException(TraductorManager_08YS.Instance.GetTexto("bll_val_sin_componentes"));
        }
    }
}
