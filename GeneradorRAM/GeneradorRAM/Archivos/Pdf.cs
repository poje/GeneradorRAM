using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GeneradorRAM.Interfaces;

namespace GeneradorRAM.Archivos
{
    public class Pdf : IArchivo
    {
        public Pdf()
        {

        }

        public string Generar(string nombreReporte, DataTable source, bool ramAntigua, string finalPath)
        {
            string fileName = string.Empty;

            ExportOptions crExportOptions = new ExportOptions();
            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            ReportDocument crReporte = new ReportDocument();

            Console.WriteLine(string.Format("Generando RAM correspondiente a Folio {0}", source.Rows[0]["Folio"]));

            if (ramAntigua)
            {
                //fileName = finalPath + ".pdf";
                fileName = string.Format(@"{0}\RAM_{1}.pdf", finalPath, source.Rows[0]["Folio"].ToString());
            }
            else
            {
                fileName = Properties.Settings.Default.PathRAMTemp + "\\" +
                DateTime.Now.Day +
                DateTime.Now.Month +
                DateTime.Now.Year +
                DateTime.Now.Minute +
                DateTime.Now.Second + ".pdf";

            }
            

            var pathReporte = Properties.Settings.Default.RptPath + "\\" + nombreReporte;
            crReporte.Load(pathReporte);
            crReporte.SetDataSource(source);
            crReporte.Refresh();

            Console.WriteLine(string.Format("Guardando en disco RAM correspondiente a Folio {0}", source.Rows[0]["Folio"]));

            crDiskFileDestinationOptions.DiskFileName = fileName;
            crExportOptions = crReporte.ExportOptions;
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

            crReporte.Export();
            crReporte.Dispose();

            crReporte.Close();

            Console.WriteLine(string.Format("RAM Folio {0} Generado", source.Rows[0]["Folio"]));


            return fileName;
        }

        public string Unir(List<string> pdfs)
        {
            throw new NotImplementedException();
        }
    }
}
