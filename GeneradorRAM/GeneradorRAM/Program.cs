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
            Ram ram = new Ram();
            Pdf archivo = new Pdf();

            Console.WriteLine("Creando Carpeta Temporal");
            carpeta.Crear();

            List<SqlParameter> list = new List<SqlParameter>();

            int codProyecto = 1010168;
            int mesAno = 202001;

            var datosRam = ram.ObtenerDatosRam(codProyecto, mesAno);

            var esCierreNuevo = ram.EsCierreNuevo(codProyecto);
            var usaRamNominas = ram.UsaRamNominas(codProyecto, mesAno);

            var codModeloIntervencion = Convert.ToInt32(datosRam.Tables[0].Rows[0]["CodModeloIntervencion"].ToString());

            var pdfs = new List<string>();

            //Generación de Pdfs según condiciones
            if (esCierreNuevo && usaRamNominas)
            {
                pdfs.Add(archivo.Generar(config.ResumenAtencionMensualFinal, datosRam.Tables[0]));

                if (datosRam.Tables.Count == 2)
                {
                    pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, datosRam.Tables[1]));
                }

                if (datosRam.Tables.Count == 3)
                {
                    if (datosRam.Tables[2].Rows[0]["Listado"].ToString() == "Listado80bis")
                    {
                        pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, datosRam.Tables[2]));
                    }

                    if (datosRam.Tables[2].Rows[0]["Listado"].ToString() == "ListadoSobreAtencion")
                    {
                        pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, datosRam.Tables[3]));

                    }
                }

                if (datosRam.Tables.Count == 4)
                {
                    pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, datosRam.Tables[1]));
                    pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, datosRam.Tables[2]));
                    pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, datosRam.Tables[3]));
                }
            }
            else
            {
                if (codModeloIntervencion == 83)
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualPAD, datosRam.Tables[0]));
                else if (codModeloIntervencion == 128)
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualPJC, datosRam.Tables[0]));
                else
                    pdfs.Add(archivo.Generar(config.ResumenAtencionMensualOriginal, datosRam.Tables[0]));
            }

            //Unir Pdf


            Console.ReadKey();
        }
    }
}
