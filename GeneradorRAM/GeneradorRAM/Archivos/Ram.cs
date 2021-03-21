using GeneradorRAM.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorRAM.Archivos
{
    public class Ram
    {
        private readonly Conexiones _conexiones;
        public Ram()
        {
            _conexiones = new Conexiones();
        }
        public bool EsCierreNuevo(int codProyecto)
        {
            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter("@CodProyecto", codProyecto));

            return Convert.ToBoolean(_conexiones.EjecutaSpScalar("GetValidacionCierre", list));
        }

        public bool UsaRamNominas(int codProyecto, int periodo)
        {
            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            list.Add(new SqlParameter("@periodo", periodo));

            return Convert.ToBoolean(_conexiones.EjecutaSpScalar("GetUsaRamNueva", list));
        }

        public DataSet ObtenerDatosRam(int codProyecto, int periodo)
        {
            List<SqlParameter> list = new List<SqlParameter>();

            list.Add(new SqlParameter("@CodProyecto", codProyecto));
            list.Add(new SqlParameter("@periodo", periodo));

            return _conexiones.EjecutaSpToDataSet("cierre_resumenatencionmensual_Firma_impresion", list);
        }
    }
}
