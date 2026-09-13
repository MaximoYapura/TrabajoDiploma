using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service_08YS
{
    public interface IIdiomaObserver_08YS
    {
        void UpdateIdioma();
    }

    public class TraductorManager_08YS
    {
        private static TraductorManager_08YS _instance;
        private readonly List<IIdiomaObserver_08YS> _observadores = new List<IIdiomaObserver_08YS>();
        private Dictionary<string, Dictionary<string, string>> _traducciones;
        private string _idiomaActual = "es"; // Idioma por defecto

        private TraductorManager_08YS()
        {
            CargarJson();
        }

        public static TraductorManager_08YS Instance
            => _instance ?? (_instance = new TraductorManager_08YS());

        private void CargarJson()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idiomas.json");

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    _traducciones = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                    
                }
                else
                {
                    _traducciones = new Dictionary<string, Dictionary<string, string>>();
                }
            }
            catch (Exception)
            {
              
                _traducciones = new Dictionary<string, Dictionary<string, string>>();
            }
        }

        public void Suscribir(IIdiomaObserver_08YS observador)
        {
            if (!_observadores.Contains(observador))
                _observadores.Add(observador);
        }

        public void Desuscribir(IIdiomaObserver_08YS observador)
        {
            _observadores.Remove(observador);
        }

        public void CambiarIdioma(string codigoIdioma)
        {
            if (_traducciones.ContainsKey(codigoIdioma))
            {
                _idiomaActual = codigoIdioma;
                Notificar();
            }
        }

        private void Notificar()
        {
            foreach (var obs in _observadores)
            {
                obs.UpdateIdioma();
            }
        }

        // Función auxiliar para recuperar un texto por su clave
        public string GetTexto(string clave)
        {
            var idiomas = _traducciones.Keys.ToList();
            if (_traducciones.ContainsKey(_idiomaActual) && _traducciones[_idiomaActual].ContainsKey(clave))
            {
                return _traducciones[_idiomaActual][clave];
            }
            return $"[{clave}]"; 
        }
    }
}
