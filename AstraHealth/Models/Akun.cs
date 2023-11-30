using System;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class Akun
    {
        private readonly string _connectionString;
        
        private readonly SqlConnection _connection;

        public Akun(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _connection = new SqlConnection(_connectionString);
        }

        public AkunModel getDataByUsernamePassword(string username, string password)
        {
            AkunModel akunModel = new AkunModel();
            try
            {
                string query = "SELECT * from ahl_msakun where akn_id = @p1 AND password=@p2";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", username);
                command.Parameters.AddWithValue("@p2", password);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    akunModel.akn_id = Convert.ToInt32(reader["akn_id"]);
                    akunModel.akn_nama = reader["akn_nama"].ToString();
                    akunModel.akn_password = reader["akn_password"].ToString();
                    akunModel.akn_role = reader["akn_role"].ToString();
                    akunModel.akn_status = reader["akn_status"].ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally 
            {
                _connection.Close();
            }
            return akunModel;
        }
    }
}
