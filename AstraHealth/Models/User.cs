using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class User
    {
        private readonly string _connectionString;

        private readonly SqlConnection _connection;

        public User(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _connection = new SqlConnection(_connectionString);
        }

        public UserModel getDataByUsername_Password(string username, string password)
        {
            UserModel UserModel = new UserModel();
            try
            {
                string query = "SELECT * FROM [user] WHERE username = @p1 AND password = @p2";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", username);
                command.Parameters.AddWithValue("@p2", password);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    UserModel.username = reader["username"].ToString();
                    UserModel.password = reader["password"].ToString();       
                    reader.Close();
                }
                else
                {
                    UserModel = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " cari User");
            }
            finally
            {
                _connection.Close();
            }
            return UserModel;
        }

        public List<UserModel> getAllData()
        {
            List<UserModel> UserList = new List<UserModel>();
            try
            {
                string query = "select * from [user]";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel User = new UserModel
                    {
                        username = reader["username"].ToString(),
                        password = reader["password"].ToString(),
                    };
                    UserList.Add(User);
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Get all " + ex.Message);
            }
            finally
            {
                _connection.Close(); // Pastikan selalu menutup koneksi, bahkan jika terjadi kesalahan
            }
            return UserList;
        }
    }
}
