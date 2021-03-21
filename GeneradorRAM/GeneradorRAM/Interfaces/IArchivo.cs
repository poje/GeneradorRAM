using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorRAM.Interfaces
{
    public interface IArchivo
    {
        string Generar(string nombreReporte, DataTable source);
    }
}
