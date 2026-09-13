using DAL_08YS.Interfaces_Repositories;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_08YS.Entities.Bitacora;
using Service_08YS;

namespace BLL_08YS
{
    public class FamiliaBLL_08YS : AccesoBLL_08YS
    {
        private readonly BitacoraBLL_08YS _bitacoraBll;
        private readonly IFamiliaRepository_08YS _familiaRepo;
        private readonly IRolRepository_08YS _rolRepo;

        public FamiliaBLL_08YS(IFamiliaRepository_08YS familiaRepo, IRolRepository_08YS rolRepo, IPermisoRepository_08YS permisoRepo, BitacoraBLL_08YS bitacoraBll) : base(permisoRepo)
        {
            _familiaRepo = familiaRepo;
            _bitacoraBll = bitacoraBll;
            _rolRepo = rolRepo;
        }

        public List<Familia_08YS> GetAll() => _familiaRepo.GetAllRoots();

        /// <summary>
        /// FormAM lado derecho (disponibles):
        /// <para>familiaIdExcluir null  → Alta: sin exclusiones por ciclos</para>
        /// <para>familiaIdExcluir int   → Modificacion: excluye la familia actual + sus contenedoras</para>
        /// <para>Los ya asignados (DGV izquierdo) los filtra la GUI quitándolos del DGV derecho</para>
        /// </summary>
        public List<AccessComponent_08YS> GetComponentesDisponibles(int? familiaIdExcluir = null)
        {
            var permisos = _permisoRepo.GetAll().Cast<AccessComponent_08YS>();
            var familias = _familiaRepo.GetAllRoots().Cast<AccessComponent_08YS>().ToList();

            if (familiaIdExcluir.HasValue)
            {
                familias = familias             // Excluir la familia que se está editando
                    .Where(f => ((Familia_08YS)f).FamiliaID != familiaIdExcluir.Value)
                    .ToList();

                // Excluir familias que ya contienen a la que se edita (evitando ciclos transitivos)
                var contenedoras = ResolverContenedoras(familiaIdExcluir.Value,
                    familias.Cast<Familia_08YS>().ToList());

                familias = familias
                    .Where(f => !contenedoras.Contains(((Familia_08YS)f).FamiliaID))
                    .ToList();
            }

            return permisos.Concat(familias).ToList();
        }

        /// <summary>
        /// Obtiene todas las familias contenedoras (directas e indirectas)
        /// de una familia determinada.
        /// </summary>
        /// <returns>
        /// Conjunto de IDs de las familias que contienen a la familia indicada,
        /// ya sea de forma directa o a través de otros niveles de jerarquía.
        /// </returns>
        private HashSet<int> ResolverContenedoras(int familiaId, List<Familia_08YS> todasFamilias)
        {
            // Almacena los IDs de las familias contenedoras encontradas. Utilizando HashSet para evitar duplicados.
            var contenedoras = new HashSet<int>();

            // Cola utilizada para recorrer la jerarquia hacia arriba mediante una busqueda en anchura (BFS).
            var cola = new Queue<int>();

            // Comienza la búsqueda desde la familia indicada.
            cola.Enqueue(familiaId);

            while (cola.Count > 0)
            {
                // Obtiene la siguiente familia a buscar de la cola.
                int buscado = cola.Dequeue();

                // Recorre todas las familias para encontrar cuales contienen a la familia actualmente buscada.
                foreach (var f in todasFamilias)
                {
                    // Verifica si alguno de los hijos de la familia actual coincide con el ID buscado.
                    bool contieneAlBuscado = f.Hijos
                        .OfType<Familia_08YS>()
                        .Any(h => h.FamiliaID == buscado);

                    // Si la familia contiene al buscado y todavia no fue agregada:
                    if (contieneAlBuscado && contenedoras.Add(f.FamiliaID))
                    {
                        // Se agrega a la cola para seguir buscando familias que contengan a esta nueva contenedora.
                        cola.Enqueue(f.FamiliaID);
                    }
                }
            }

            // Devuelve todas las familias contenedoras encontradas.
            return contenedoras;
        }

        public void Crear(string nombre, HashSet<AccessComponent_08YS> componentes)
        {
            ValidarDatosEntrada(nombre, componentes);
            ValidarFamiliaNoExistente(nombre, componentes, null);

            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearFamilias);
            _familiaRepo.Create(nombre, componentes.ToList());
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.FamiliaCreada);
        }

        public void Modificar(int familiaId, string nombre, HashSet<AccessComponent_08YS> componentes)
        {
            ValidarDatosEntrada(nombre, componentes);
            ValidarFamiliaNoExistente(nombre, componentes, familiaId);
            ValidarPropagacion(familiaId, componentes, out var rolesAncestrosIds);

            SessionManager_08YS.Instance.ValidatePermission(Permisos.ModificarFamilias);
            _familiaRepo.Modify(familiaId, nombre, componentes.ToList());
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.FamiliaModificada);

            int rolActual = SessionManager_08YS.Instance.Current.Rol.RolID;
            if(rolesAncestrosIds.Contains(rolActual))
                SessionManager_08YS.Instance.InvalidarSesion();
        }

        /// <summary>
        /// Verifica que modificar esta familia no genere permisos duplicados en
        /// ningún contenedor (familia o rol) que dependa de ella, directa o transitivamente.
        /// </summary>
        private void ValidarPropagacion(int familiaId, HashSet<AccessComponent_08YS> nuevaComposicion, out List<int> rolesAncestrosIds)
        {
            var (familiaIds, rolIds) = _familiaRepo.GetAncestors(familiaId);
            rolesAncestrosIds = rolIds;
            if (!familiaIds.Any() && !rolIds.Any())
                return;

            var familiasDict = _familiaRepo.GetAllDictionary();

            if (familiasDict.TryGetValue(familiaId, out var familiaEditada))
                familiaEditada.ReemplazarHijos(nuevaComposicion);

            var conflictos = new List<string>();

            string plantillaConflictoLinea = TraductorManager_08YS.Instance.GetTexto("bll_propagacion_conflicto_linea");

            foreach (int fid in familiaIds)
                if (familiasDict.TryGetValue(fid, out var f))
                    conflictos.AddRange(
                        DetectarConflictosEntreHijos(f.Nombre, "Familia", f.Hijos, plantillaConflictoLinea));

            if (rolIds.Any())
            {
                var rolesAfectados = _rolRepo.GetAll(familiasDict)
                    .Where(r => rolIds.Contains(r.RolID));

                foreach (var r in rolesAfectados)
                    conflictos.AddRange(
                        DetectarConflictosEntreHijos(r.Nombre, "Rol", r.Componentes, plantillaConflictoLinea));
            }

            if (conflictos.Any())
            {
                string plantillaEncabezado = TraductorManager_08YS.Instance.GetTexto("bll_propagacion_conflicto_encabezado");
                throw new InvalidOperationException(
                    plantillaEncabezado + "\n\n" + string.Join("\n", conflictos));
            }
        }

        private List<string> DetectarConflictosEntreHijos(
            string nombreContenedor, string tipo,
            IReadOnlyList<AccessComponent_08YS> hijos, string plantillaLinea)
        {
            var resultado = new List<string>();

            // ── PASO 1: Precalculo ──────────────────────────────────────────────────
            // Por cada hijo, recorremos una unica vez todo su árbol con GetPermisos()
            // y guardamos el resultado (solo los IDs, no los objetos completos) en un
            // diccionario. Usamos el objeto AccessComponent_08YS como clave porque acá
            // todos son instancias reales de la misma lista (no copias), entonces la
            // igualdad por referencia que usa Dictionary lo resuelve con ese objeto concreto".
            var permisosPorHijo = hijos.ToDictionary(
                hijo => hijo,
                hijo => hijo.GetPermisos()
                            .Select(p => p.PermisoID)
                            .ToHashSet());

            // ── PASO 2: Comparación par a par ────────────────────────────────────────
            // Recorremos todos los pares posibles (i, j) sin repetir combinaciones.
            // j siempre arranca en i+1 para no comparar un hijo consigo mismo
            // ni repetir el par (B,A) si ya comparamos (A,B).
            for (int i = 0; i < hijos.Count; i++)
            {
                for (int j = i + 1; j < hijos.Count; j++)
                {
                    var hijoA = hijos[i];
                    var hijoB = hijos[j];

                    // Buscamos los HashSets ya calculados en el paso 1.
                    // Esto es una lectura de diccionario (O(1)), no un recorrido.
                    var permisosA = permisosPorHijo[hijoA];
                    var permisosB = permisosPorHijo[hijoB];

                    // Overlaps() devuelve true si hay al menos un elemento en común
                    // entre los dos HashSets, sin necesidad de intersectarlos
                    // completamente — corta apenas encuentra la primera coincidencia.
                    if (permisosA.Overlaps(permisosB))
                    {
                        resultado.Add(
                            string.Format(plantillaLinea, tipo, nombreContenedor, hijoA.Nombre, hijoB.Nombre));
                    }
                }
            }

            return resultado;
        }

        public void Eliminar(int familiaId)
        {
            if (_familiaRepo.IsInUse(familiaId))
                throw new ComponenteEnUsoException_08YS();

            SessionManager_08YS.Instance.ValidatePermission(Permisos.EliminarFamilias);
            _familiaRepo.Delete(familiaId);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.FamiliaEliminada);
        }

        private void ValidarFamiliaNoExistente(string nombre, HashSet<AccessComponent_08YS> componentes, int? excluirId)
        {
            var permsNuevos = componentes.SelectMany(c => c.GetPermisos()).Select(p => p.PermisoID).ToHashSet();
            var familias = _familiaRepo.GetAllRoots().Where(f => !excluirId.HasValue || f.FamiliaID != excluirId.Value).ToList();

            if (familias.Any(f => f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new NombreDuplicadoException_08YS();

            if (familias.Any(f => f.GetPermisos().Select(p => p.PermisoID).ToHashSet().SetEquals(permsNuevos)))
                throw new PermisosDuplicadosException_08YS();
        }
    }
}
