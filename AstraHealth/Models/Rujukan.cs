using System;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class Rujukan
    {
        private readonly string _connectionString;

        private readonly SqlConnection _connection;

        public Rujukan(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _connection = new SqlConnection(_connectionString);
        }

        public List<RujukanModel> getAllData()
        {
            List<RujukanModel> rujukanList = new List<RujukanModel>();
            try
            {
                string query = "select * from ahl_trrujukan";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RujukanModel rujukan = new RujukanModel
                    {
                        rjk_id = Convert.ToInt32(reader["rjk_id"]),
                        rjk_id_registrasi = Convert.ToInt32(reader["rjk_id_registrasi"]),
                        rjk_rumah_sakit = reader["rjk_rumah_sakit"].ToString(),
                        rjk_tanggal = Convert.ToInt32(reader["rjk_tanggal"]),
                        rjk_keterangan = reader["rjk_keterangan"].ToString(),
                    };
                    rujukanList.Add(rujukan);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rujukanList;
        }
    }
}
