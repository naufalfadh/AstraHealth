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

        public AkunModel getDataByUsernamePassword(string akn_id, string akn_password)
        {
            AkunModel akunModel = new AkunModel();
            try
            {
                string query = "SELECT * from ahl_msakun where akn_id = @p1 AND akn_password=@p2";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", akn_id);
                command.Parameters.AddWithValue("@p2", akn_password);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    akunModel.akn_id = reader["akn_id"].ToString();
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

        public List<AkunModel> getAllData()
        {
            List<AkunModel> akunList = new List<AkunModel>();
            try
            {
                string query = "select * from ahl_msakun";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AkunModel akun= new AkunModel
                    {
                        akn_id = reader["akn_id"].ToString(),
                        akn_nama = reader["akn_nama"].ToString(),
                        akn_password = reader["akn_password"].ToString(),
                        akn_role = reader["akn_role"].ToString(),
                        akn_status = reader["akn_status"].ToString(),
                    };
                    akunList.Add(akun);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return akunList;
        }

        public AkunModel getData(string id)
        {
            AkunModel akunModel = new AkunModel();
            try
            {
                string query = "select * from ahl_msakun where akn_id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                akunModel.akn_id = reader["akn_id"].ToString();
                akunModel.akn_nama = reader["akn_nama"].ToString();
                akunModel.akn_password = reader["akn_password"].ToString();
                akunModel.akn_role = reader["akn_role"].ToString();
                akunModel.akn_status = reader["akn_status"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return akunModel;
        }

        public void insertData(AkunModel akunModel)
        {
            try
            {
                string query = "insert into ahl_msakun values(@p1, @p2, @p3, @p4, @p5)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", akunModel.akn_id);
                command.Parameters.AddWithValue("@p2", akunModel.akn_nama);
                command.Parameters.AddWithValue("@p3", akunModel.akn_password);
                command.Parameters.AddWithValue("@p4", akunModel.akn_role);
                command.Parameters.AddWithValue("@p5", akunModel.akn_status);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateData(AkunModel akunModel)
        {
            try
            {
                string query = "update ahl_msakun " +
                "set akn_nama = @p2, " +
                "akn_password = @p3, " +
                "akn_role = @p4, " +
                "akn_status = @p5 " +
                "where akn_id = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", akunModel.akn_id);
                command.Parameters.AddWithValue("@p2", akunModel.akn_nama);
                command.Parameters.AddWithValue("@p3", akunModel.akn_password);
                command.Parameters.AddWithValue("@p4", akunModel.akn_role);
                command.Parameters.AddWithValue("@p5", akunModel.akn_status);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void deleteData(string id)
        {
            try
            {
                string query = "delete from ahl_msakun where akn_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
