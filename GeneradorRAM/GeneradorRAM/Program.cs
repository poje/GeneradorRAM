using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using GeneradorRAM.Helpers;
using GeneradorRAM.Interfaces;
using GeneradorRAM.Archivos;

namespace GeneradorRAM
{
    class Program
    {
        static void Main(string[] args)
        {
            Carpeta carpeta = new Carpeta();

            Console.WriteLine("Creando Carpeta Temporal");
            carpeta.Crear();

            List<SqlParameter> list = new List<SqlParameter>();

            int codProyecto = 1010168;
            int mesAno = 201901;

            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            list.Add(new SqlParameter("@MesAno", mesAno));

            Conexiones c = new Conexiones();
            var ds = c.EjecutaSpToDataSet("cierre_resumenatencionmensual_Firma_impresion", list);
            var archivo = new Pdf();

            archivo.Generar(Properties.Settings.Default.ResumenAtencionMensualFinal, ds.Tables[0]);
            archivo.Generar(Properties.Settings.Default.ResumenListadoPlazasConvenidas, ds.Tables[1]);
            archivo.Generar(Properties.Settings.Default.ResumenListadoNNA80bis, ds.Tables[2]);

            Console.WriteLine("Obteniendo Fechas de Enviado");
            Console.WriteLine("Generar Carpetas por Fecha de Envio");
            Console.WriteLine("Generar Archivos en carpetas de envío");
            Console.WriteLine("Insertar en base de datos el archivo creado");
            Console.WriteLine("Insertar en base de datos en tabla de archivos");

            Console.WriteLine("Eliminando carpeta temporal");
            


            Console.ReadKey();
        }
    }
}
