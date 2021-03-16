using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneradorRAM.Helpers
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository()
        {
            _connectionString = Properties.Settings.Default.ConnectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
