using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AngularPerformanceSamples.Services
{
    public class SqlServerClient
    {
        private string connectionString;
        public SqlServerClient(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<object> ExecuteNonQuery(string query)
        {
            return await ExecuteNonQuery(new SqlCommand(query));
        }
        public async Task<object> ExecuteNonQuery(SqlCommand command)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                command.Connection = con;
                using (command)
                {
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<object> ExecuteReader(string query)
        {
            return await ExecuteReader(new SqlCommand(query));
        }
        public async Task<object> ExecuteReader(SqlCommand command)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                command.Connection = con;
                using (command)
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            return reader.GetString(1);
                        }
                    }
                }
            }
            return null;
        }
    }
}
