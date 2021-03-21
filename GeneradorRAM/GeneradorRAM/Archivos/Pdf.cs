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

        public string Generar(string nombreReporte, DataTable source)
        {
            string fileName = string.Empty;

            ExportOptions crExportOptions = new ExportOptions();
            DiskFileDestinationOptions crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            ReportDocument crReporte = new ReportDocument();

            fileName = Properties.Settings.Default.PathRAMTemp + "\\" + 
                DateTime.Now.Day + 
                DateTime.Now.Month + 
                DateTime.Now.Year + 
                DateTime.Now.Minute +
                DateTime.Now.Second + ".pdf";

            var pathReporte = Properties.Settings.Default.RptPath + "\\" + nombreReporte;
            crReporte.Load(pathReporte);
            crReporte.SetDataSource(source);
            crReporte.Refresh();
            
            crDiskFileDestinationOptions.DiskFileName = fileName;
            crExportOptions = crReporte.ExportOptions;
            crExportOptions.DestinationOptions = crDiskFileDestinationOptions;
            crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

            crReporte.Export();
            crReporte.Dispose();

            crReporte.Close();

            return fileName;
        }

        public string Unir(List<string> pdfs)
        {
            throw new NotImplementedException();
        }
    }
}
