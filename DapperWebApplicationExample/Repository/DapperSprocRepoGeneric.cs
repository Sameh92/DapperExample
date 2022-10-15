using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperWebApplicationExample.Repository
{
    public class DapperSprocRepoGeneric : IDapperSprocRepoGeneric
    {
        private IConfiguration _configuration { get; set; }

        public DapperSprocRepoGeneric(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public string ConnectionString
        {
            get; set;
        }


        public void Execute(string name)
        {
            Execute(name, null);
        }


        public void Execute(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Execute(name, param, commandType: CommandType.StoredProcedure);
            }
        }




        public T SingleByProcedureNameAndId<T>(string name, int id)
        {
            return SingleByProcedureName<T>(name, new { id });
        }
        public T SingleByProcedureName<T>(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                var result = cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.FirstOrDefault();
            }

            return default(T);
        }




        public List<T> List<T>(string name, int id)
        {
            return List<T>(name, new { id });
        }

        public List<T> List<T>(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {

                var result = cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.ToList();
            }

            return new List<T>();
        }




        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                var result = cnn.QueryMultiple(name, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();



                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                var result = cnn.QueryMultiple(name, param, commandType: CommandType.StoredProcedure);

                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                var item3 = result.Read<T3>().ToList();

                if (item1 != null && item2 != null && item3 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
            }

            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(new List<T1>(), new List<T2>(), new List<T3>());
        }        
        public List<T> ListByProcedureName<T>(string procName)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                var result = cnn.Query<T>(procName, commandType: CommandType.StoredProcedure);

                if (result != null)
                    return result.ToList();
            }
            return new List<T>();
        }


        public void QueryExecute(string name, object param)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Execute(name, param, commandType: CommandType.Text);
            }
        }

        public void QueryExecute(string name)
        {
            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Execute(name, null, commandType: CommandType.Text);
            }
        }
    }
}
