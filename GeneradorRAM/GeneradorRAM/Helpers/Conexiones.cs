using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorRAM.Helpers
{
    public class Conexiones : Repository
    {
        public Conexiones()
        {

        }

        public int EjecutaSP(string nombreProcedimiento)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        public int EjecutaSP(string nombreProcedimiento, List<SqlParameter> parametros)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }

                    var rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
        }
        public int EjecutaSpScalar(string nombreProcedimiento)
        {

            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var scalarValue = Convert.ToInt32(cmd.ExecuteScalar());

                    return scalarValue;
                }
            }
        }
        public int EjecutaSpScalar(string nombreProcedimiento, List<SqlParameter> parametros)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }

                    var scalarValue = Convert.ToInt32(cmd.ExecuteScalar());

                    return scalarValue;
                }
            }
        }
        public string EjecutaSpScalarString(string nombreProcedimiento)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var scalarValue = cmd.ExecuteScalar().ToString();

                    return scalarValue;
                }
            }
        }
        public string EjecutaSpScalarString(string nombreProcedimiento, List<SqlParameter> parametros)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }

                    var scalarValue = cmd.ExecuteScalar().ToString();

                    return scalarValue;
                }
            }
        }
        public DataTable EjecutaSpToDataTable(string nombreProcedimiento)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    DataTable dt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sqlda = new SqlDataAdapter(cmd);

                    sqlda.Fill(dt);

                    return dt;
                }
            }
        }
        public DataTable EjecutaSpToDataTable(string nombreProcedimiento, List<SqlParameter> parametros)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    DataTable dt = new DataTable();

                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (SqlParameter parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }

                    SqlDataAdapter sqlda = new SqlDataAdapter(cmd);

                    sqlda.Fill(dt);

                    return dt;
                }
            }
        }
        public DataSet EjecutaSpToDataSet(string nombreProcedimiento)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sqlda = new SqlDataAdapter(cmd);

                    sqlda.Fill(ds);

                    return ds;
                }
            }
        }
        public DataSet EjecutaSpToDataSet(string nombreProcedimiento, List<SqlParameter> parametros)
        {
            using (var con = GetConnection())
            {
                con.Open();

                using (var cmd = new SqlCommand(nombreProcedimiento, con))
                {
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.Add(parametro);
                    }
                    var sqlda = new SqlDataAdapter(cmd);
                    sqlda.Fill(ds);
                    return ds;

                }
            }
        }
    }
}
