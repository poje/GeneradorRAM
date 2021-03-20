using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GeneradorRAM.Helpers
{
    public class Carpeta
    {
        public Carpeta()
        {
            
        }

        public void Crear()
        {
            if (!Directory.Exists(Properties.Settings.Default.PathRAMTemp))
            {
                Directory.CreateDirectory(Properties.Settings.Default.PathRAMTemp);
            }
        }

        public void Eliminar()
        {
            Directory.Delete(Properties.Settings.Default.PathRAMTemp);
        }
    }
}
