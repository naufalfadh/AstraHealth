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
                        kpo_tanggal_aksi = Convert.ToDateTime(reader["kpo_tanggal_aksi"]),
                        kpo_status = reader["kpo_status"].ToString(),
                        kpo_catatan = reader["kpo_catatan"].ToString(),
                        kpo_satuan = reader["kpo_satuan"].ToString(),
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

        public ObatModel getData(int id)
        {
            ObatModel obatModel = new ObatModel();
            try
            {
                string query = "select * from ahl_trkeperluanObat where kpo_id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                obatModel.kpo_id = Convert.ToInt32(reader["kpo_id"].ToString());
                obatModel.kpo_nama_obat = reader["kpo_nama_obat"].ToString();
                obatModel.kpo_jumlah = Convert.ToInt32(reader["kpo_jumlah"].ToString());
                obatModel.kpo_tanggal_pengajuan = Convert.ToDateTime(reader["kpo_tanggal_pengajuan"].ToString());
                obatModel.kpo_tanggal_aksi = Convert.ToDateTime(reader["kpo_tanggal_aksi"].ToString());
                obatModel.kpo_status = reader["kpo_status"].ToString();
                obatModel.kpo_catatan = reader["kpo_catatan"].ToString();
                obatModel.kpo_id_admin = reader["kpo_id_admin"].ToString();
                obatModel.kpo_satuan = reader["kpo_satuan"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obatModel;
        }

        public void insertData(ObatModel obatModel)
        {
            try
            {
                string query = "insert into ahl_trkeperluanObat values(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", obatModel.kpo_nama_obat);
                command.Parameters.AddWithValue("@p2", obatModel.kpo_jumlah);
                command.Parameters.AddWithValue("@p3", obatModel.kpo_tanggal_pengajuan);
                command.Parameters.AddWithValue("@p4", obatModel.kpo_tanggal_aksi);
                command.Parameters.AddWithValue("@p5", obatModel.kpo_status);
                command.Parameters.AddWithValue("@p6", obatModel.kpo_catatan);
                command.Parameters.AddWithValue("@p7", obatModel.kpo_id_admin);
                command.Parameters.AddWithValue("@p8", obatModel.kpo_satuan);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateData(ObatModel obatModel)
        {
            try
            {
                string query = "update ahl_trkeperluanObat " +
                "set kpo_catatan = @p2 " +
                "where kpo_id = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", obatModel.kpo_id);
                command.Parameters.AddWithValue("@p2", obatModel.kpo_catatan);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void acceptData(int id)
        {
            try
            {
                string query = "update ahl_trkeperluanObat set kpo_status='diterima', kpo_tanggal_aksi=@p2 where kpo_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", DateTime.Now);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void rejectData(int id)
        {
            try
            {
                string query = "update ahl_trkeperluanObat set kpo_status='ditolak', kpo_tanggal_aksi=@p2 where kpo_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", DateTime.Now);
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
