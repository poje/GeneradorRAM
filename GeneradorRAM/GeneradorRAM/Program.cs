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
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Drawing;

namespace GeneradorRAM
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Properties.Settings.Default;
            Ram ram = new Ram();
            Pdf archivo = new Pdf();

            CarpetaArchivo carpeta = new CarpetaArchivo();

            Console.WriteLine("Creando Carpeta Temporal");
            carpeta.CrearCarpetaTemporal();
            Console.WriteLine("Creando Carpeta TemporalGenerada");

            foreach (DataRow rowEnvio in ram.PeriodosEnviados.Rows)
            {
                string ano = rowEnvio["Ano"].ToString();
                string nombreCarpetaFechaEnviado = rowEnvio["FechaNombreCarpeta"].ToString();
                
                var ramEnvio = ram.ObtenerRamsxFechaEnvio(rowEnvio["FechaEnvio"].ToString());

                string path = string.Format(@"C:\Ram-Fes\{0}\{1}\", ano, nombreCarpetaFechaEnviado);
                carpeta.Crear(path);

                foreach (DataRow datosRamEnvio in ramEnvio.Rows)
                {
                    int codProyecto = Convert.ToInt32(datosRamEnvio["CodProyecto"].ToString());
                    int mesAno = Convert.ToInt32(datosRamEnvio["MesAno"].ToString());

                    var datosRam = ram.ObtenerDatosRam(codProyecto, mesAno);
                    var esCierreNuevo = ram.EsCierreNuevo(codProyecto);
                    var usaRamNominas = ram.UsaRamNominas(codProyecto, mesAno);

                    var codModeloIntervencion = Convert.ToInt32(datosRam.Tables[0].Rows[0]["CodModeloIntervencion"].ToString());

                    var pdfs = new List<string>();

                    var finalPath = string.Format(@"{0}\{1}", path, datosRamEnvio["MesAno"].ToString()); 

                    
                    carpeta.Crear(finalPath);

                    //Generación de Pdfs según condiciones
                    if (esCierreNuevo && usaRamNominas)
                    {
                        pdfs.Add(archivo.Generar(config.ResumenAtencionMensualFinal, datosRam.Tables[0], false, string.Empty));

                        if (datosRam.Tables.Count == 2)
                        {
                            pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, datosRam.Tables[1], false, string.Empty));
                        }

                        if (datosRam.Tables.Count == 3)
                        {
                            if (datosRam.Tables[2].Rows[0]["Listado"].ToString() == "Listado80bis")
                            {
                                pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, datosRam.Tables[2], false, string.Empty));
                            }

                            if (datosRam.Tables[2].Rows[0]["Listado"].ToString() == "ListadoSobreAtencion")
                            {
                                pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, datosRam.Tables[2], false, string.Empty));
                            }
                        }

                        if (datosRam.Tables.Count == 4)
                        {
                            pdfs.Add(archivo.Generar(config.ResumenListadoPlazasConvenidas, datosRam.Tables[1], false, string.Empty));
                            pdfs.Add(archivo.Generar(config.ResumenListadoNNA80bis, datosRam.Tables[2], false, string.Empty));
                            pdfs.Add(archivo.Generar(config.ResumenListadoNNASobreAtencion, datosRam.Tables[3], false, string.Empty));
                        }

                        //unir PDFs

                        PdfDocument pdfFinal = new PdfDocument();

                        foreach (var pdfReady in pdfs)
                        {
                            PdfDocument pdfInputDocument = PdfReader.Open(pdfReady, PdfDocumentOpenMode.Import);

                            int count = pdfInputDocument.PageCount;

                            for (int idx = 0; idx < count; idx++)
                            {
                                // Get the page from the external document...
                                PdfPage page = pdfInputDocument.Pages[idx];

                                // ...and add it to the output document.
                                pdfFinal.AddPage(page);
                            }
                        }

                        for (int i = 0; i < pdfFinal.Pages.Count; i++)
                        {
                            PdfPage page = pdfFinal.Pages[i];

                            XFont font = new XFont("Times New Roman", 10);
                            XBrush brush = XBrushes.Black;

                            string noPages = pdfFinal.Pages.Count.ToString();

                            XRect layoutRectangle = new XRect(0 /*X*/, page.Height - font.Height /*Y*/,
                                page.Width /*Width*/, font.Height /*Height*/);
                            //XRect layoutRectangle = new XRect(0, 0, 0, 0);

                            using (XGraphics gfx = XGraphics.FromPdfPage(page))
                            {
                                gfx.DrawString(
                                    "Página " + (i + 1) + " de " + noPages,
                                    font,
                                    brush,
                                    layoutRectangle,
                                    XStringFormats.BottomCenter);
                            }

                            foreach (var pdf in pdfs)
                            {
                                carpeta.EliminarArchivo(pdf);
                            }
                        }

                        string fileFinal = string.Format(@"{0}\RAM_{1}.pdf", finalPath, datosRam.Tables[0].Rows[0]["Folio"].ToString());
                        pdfFinal.Save(fileFinal);
                    }
                    else
                    {
                        if (codModeloIntervencion == 83)
                            archivo.Generar(config.ResumenAtencionMensualPAD, datosRam.Tables[0], true, finalPath);
                        else if (codModeloIntervencion == 128)
                            archivo.Generar(config.ResumenAtencionMensualPJC, datosRam.Tables[0], true, finalPath);
                        else
                            archivo.Generar(config.ResumenAtencionMensualOriginal, datosRam.Tables[0], true, finalPath);
                    }
                }
            }

            carpeta.EliminarCarpetaTemporal();

            Console.ReadKey();
        }
    }
}
