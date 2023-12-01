using System;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class Obat
    {
        private readonly string _connectionString;

        private readonly SqlConnection _connection;

        public Obat(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _connection = new SqlConnection(_connectionString);
        }

        public List<ObatModel> getAllData()
        {
            List<ObatModel> obatList = new List<ObatModel>();
            try
            {
                string query = "select * from ahl_trkeperluanObat";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ObatModel obat = new ObatModel
                    {
                        kpo_id = Convert.ToInt32(reader["kpo_id"]),
                        kpo_nama_obat = reader["kpo_nama_obat"].ToString(),
                        kpo_jumlah = Convert.ToInt32(reader["kpo_jumlah"]),
                        kpo_tanggal_pengajuan = Convert.ToDateTime(reader["kpo_tanggal_pengajuan"]),
                        kpo_status = reader["akn_status"].ToString(),
                        kpo_tanggal_aksi = Convert.ToDateTime(reader["kpo_tanggal_aksi"]),
                    };
                    obatList.Add(obat);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obatList;
        }
    }
}
