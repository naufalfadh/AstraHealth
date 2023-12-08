using System;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class KeperluanMedis
    {
        private readonly string _connectionString;

        private readonly SqlConnection _connection;

        public KeperluanMedis(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

            _connection = new SqlConnection(_connectionString);
        }

        public List<KeperluanMedisModel> getAllData()
        {
            List<KeperluanMedisModel> keperluanMedisList = new List<KeperluanMedisModel>();
            try
            {
                string query = "select * from ahl_trkeperluanMedis";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    KeperluanMedisModel keperluanMedis = new KeperluanMedisModel
                    {
                        kpm_id = Convert.ToInt32(reader["kpm_id"]),
                        kpm_nama_barang = reader["kpm_nama_barang"].ToString(),
                        kpm_jumlah = Convert.ToInt32(reader["kpm_jumlah"]),
                        kpm_satuan = reader["kpm_satuan"].ToString(),
                        kpm_tanggal_pengajuan = Convert.ToDateTime(reader["kpm_tanggal_pengajuan"]),
                        kpm_tanggal_aksi = Convert.ToDateTime(reader["kpm_tanggal_aksi"]),
                        kpm_status = reader["kpm_status"].ToString(),
                        kpm_catatan = reader["kpm_catatan"].ToString(),
                    };
                    keperluanMedisList.Add(keperluanMedis);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return keperluanMedisList;
        }

        public KeperluanMedisModel getData(int id)
        {
            KeperluanMedisModel keperluanMedisModel = new KeperluanMedisModel();
            try
            {
                string query = "select * from ahl_trkeperluanMedis where kpm_id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                keperluanMedisModel.kpm_id = Convert.ToInt32(reader["kpm_id"].ToString());
                keperluanMedisModel.kpm_nama_barang = reader["kpm_nama_barang"].ToString();
                keperluanMedisModel.kpm_jumlah = Convert.ToInt32(reader["kpm_jumlah"].ToString());
                keperluanMedisModel.kpm_tanggal_pengajuan = Convert.ToDateTime(reader["kpm_tanggal_pengajuan"].ToString());
                keperluanMedisModel.kpm_tanggal_aksi = Convert.ToDateTime(reader["kpm_tanggal_aksi"].ToString());
                keperluanMedisModel.kpm_status = reader["kpm_status"].ToString();
                keperluanMedisModel.kpm_catatan = reader["kpm_catatan"].ToString();
                keperluanMedisModel.kpm_id_admin = reader["kpm_id_admin"].ToString();
                keperluanMedisModel.kpm_satuan = reader["kpm_satuan"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return keperluanMedisModel;
        }

        public void insertData(KeperluanMedisModel keperluanMedisModel)
        {
            try
            {
                string query = "insert into ahl_trkeperluanMedis values(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", keperluanMedisModel.kpm_nama_barang);
                command.Parameters.AddWithValue("@p2", keperluanMedisModel.kpm_jumlah);
                command.Parameters.AddWithValue("@p3", keperluanMedisModel.kpm_satuan);
                command.Parameters.AddWithValue("@p4", keperluanMedisModel.kpm_tanggal_pengajuan);
                command.Parameters.AddWithValue("@p5", keperluanMedisModel.kpm_tanggal_aksi);
                command.Parameters.AddWithValue("@p6", keperluanMedisModel.kpm_status);
                command.Parameters.AddWithValue("@p7", keperluanMedisModel.kpm_catatan);
                command.Parameters.AddWithValue("@p8", keperluanMedisModel.kpm_id_admin);
                command.Parameters.AddWithValue("@p9", keperluanMedisModel.kpm_id_manajer);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateData(KeperluanMedisModel keperluanMedisModel)
        {
            try
            {
                string query = "update ahl_trkeperluanMedis " +
                "set kpm_catatan = @p2 " +
                "where kpm_id = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", keperluanMedisModel.kpm_id);
                command.Parameters.AddWithValue("@p2", keperluanMedisModel.kpm_catatan);
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
                string query = "update ahl_trkeperluanMedis set kpm_status='diterima', kpm_tanggal_aksi=@p2 where kpm_id = @p1";
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
                string query = "update ahl_trkeperluanMedis set kpm_status='ditolak', kpm_tanggal_aksi=@p2 where kpm_id = @p1";
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
