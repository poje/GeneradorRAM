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

            var config = Properties.Settings.Default;
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
                pdfs.Add(archivo.Generar(config.ResumenAtencionMensualFinal, ds.Tables[0]));

                if (ds.Tables.Count == 2)
                {
                    pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, ds.Tables[1]));
                }

                if (ds.Tables.Count == 3)
                {
                    if (ds.Tables[2].Rows[0]["Listado"].ToString() == "Listado80bis")
                    {
                        pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, ds.Tables[2]));
                    }

                    if (ds.Tables[2].Rows[0]["Listado"].ToString() == "ListadoSobreAtencion")
                    {
                        pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, ds.Tables[3]));

                    }
                }

                if (ds.Tables.Count == 4)
                {
                    pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, ds.Tables[1]));
                    pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, ds.Tables[2]));
                    pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, ds.Tables[3]));
                }
            }
            else
            {
                if (codModeloIntervencion == 83)
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualPAD, ds.Tables[0]));
                else if (codModeloIntervencion == 128)
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualPJC, ds.Tables[0]));
                else
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualOriginal, ds.Tables[0]));
            }

            //Unir Pdf


            Console.ReadKey();
        }
    }
}
