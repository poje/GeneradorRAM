using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GeneradorRAM.Properties;

namespace GeneradorRAM.Helpers
{
    public class CarpetaArchivo
    {
        private Settings _config;
        public CarpetaArchivo()
        {
            _config = Settings.Default;
        }

        public void CrearCarpetaTemporal()
        {
            if (!Directory.Exists(_config.PathRAMTemp))
            {
                Directory.CreateDirectory(_config.PathRAMTemp);
                Console.WriteLine($"Carpeta {0} creada correctamente", _config.PathRAMTemp);
            }
        }

        public void Crear(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Carpeta {path} creada correctamente");
            }
            else
                Console.WriteLine($"Carpeta {path} ya existe", path);
        }

        public void EliminarArchivo(string path)
        {
            File.Delete(path);
        }

        public void EliminarCarpetaTemporal()
        {
            Directory.Delete(_config.PathRAMTemp);
        }
    }
}
