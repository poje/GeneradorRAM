using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GeneradorRAM.Helpers;

namespace GeneradorRAM
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Obteniendo Fechas de Enviado");
            Console.WriteLine("Generar Carpetas por Fecha de Envio");
            Console.WriteLine("Generar Archivos en carpetas de envío");
            Console.WriteLine("Insertar en base de datos el archivo creado");
            Console.WriteLine("Insertar en base de datos en tabla de archivos");

            Console.ReadKey();
        }
    }
}
