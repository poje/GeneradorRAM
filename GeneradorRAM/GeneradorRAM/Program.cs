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
            int mesAno = 202001;

            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            list.Add(new SqlParameter("@MesAno", mesAno));

            Conexiones c = new Conexiones();
            var ds = c.EjecutaSpToDataSet("cierre_resumenatencionmensual_Firma_impresion", list);
            var archivo = new Pdf();


            list.Clear();
            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            var esCierreNuevo = Convert.ToBoolean(c.EjecutaSpScalar("GetValidacionCierre", list));

            list.Clear();
            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            list.Add(new SqlParameter("@Periodo", mesAno));
            var usaRamNominas = Convert.ToBoolean(c.EjecutaSpScalar("GetUsaRamNueva", list));

            var codModeloIntervencion = Convert.ToInt32(ds.Tables[0].Rows[0]["CodModeloIntervencion"].ToString());

            var pdfs = new List<string>();

            //Generación de Pdfs según condiciones
            if (esCierreNuevo && usaRamNominas)
            {
                pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenAtencionMensualFinal, ds.Tables[0]));

                if (ds.Tables.Count == 2)
                {
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoPlazasConvenidas, ds.Tables[1]));
                }

                if (ds.Tables.Count == 3)
                {
                    if (ds.Tables[2].Rows[0]["Listado"].ToString() == "Listado80bis")
                    {
                        pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoNNA80bis, ds.Tables[2]));
                    }

                    if (ds.Tables[2].Rows[0]["Listado"].ToString() == "ListadoSobreAtencion")
                    {
                        pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoNNASobreAtencion, ds.Tables[3]));

                    }
                }

                if (ds.Tables.Count == 4)
                {
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoPlazasConvenidas, ds.Tables[1]));
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoNNA80bis, ds.Tables[2]));
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenListadoNNASobreAtencion, ds.Tables[3]));
                }
            }
            else
            {
                if (codModeloIntervencion == 83)
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenAtencionMensualPAD, ds.Tables[0]));
                else if (codModeloIntervencion == 128)
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenAtencionMensualPJC, ds.Tables[0]));
                else
                    pdfs.Add(archivo.Generar(Properties.Settings.Default.ResumenAtencionMensualOriginal, ds.Tables[0]));
            }

            //Unir Pdf

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
