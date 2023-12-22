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
                string query = "SELECT km.*, admin.akn_nama AS kpm_nama_admin, manajer.akn_nama AS kpm_nama_manajer " +
                       "FROM ahl_trkeperluanMedis km " +
                       "JOIN ahl_msakun admin ON km.kpm_id_admin = admin.akn_id " +
                       "JOIN ahl_msakun manajer ON km.kpm_id_manajer = manajer.akn_id " +
                       "ORDER BY km.kpm_tanggal_pengajuan DESC";
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
                        kpm_tanggal_aksi = reader["kpm_tanggal_aksi"] == DBNull.Value ? null : Convert.ToDateTime(reader["kpm_tanggal_aksi"]),
                        kpm_tanggal_diterima = reader["kpm_tanggal_diterima"] == DBNull.Value ? null : Convert.ToDateTime(reader["kpm_tanggal_diterima"]),
                        kpm_status = reader["kpm_status"] == DBNull.Value ? null : reader["kpm_status"].ToString(),
                        kpm_catatan = reader["kpm_catatan"].ToString(),
                        kpm_id_admin = reader["kpm_id_admin"].ToString(),
                        kpm_id_manajer = reader["kpm_id_manajer"].ToString(),
                        kpm_nama_admin = reader["kpm_nama_admin"].ToString(),
                        kpm_nama_manajer = reader["kpm_nama_manajer"].ToString(),
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
                keperluanMedisModel.kpm_satuan = reader["kpm_satuan"].ToString();
                keperluanMedisModel.kpm_tanggal_pengajuan = Convert.ToDateTime(reader["kpm_tanggal_pengajuan"]);
                keperluanMedisModel.kpm_status = reader["kpm_status"].ToString();
                keperluanMedisModel.kpm_id_admin = reader["kpm_id_admin"].ToString();
                keperluanMedisModel.kpm_id_manajer = reader["kpm_id_manajer"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return keperluanMedisModel;
        }

        public void insertData(KeperluanAlatMedis keperluanAlatMedis)
        {
            try
            {
                foreach (var keperluanMedis in keperluanAlatMedis.keperluanMedis)
                {
                    string query = "insert into ahl_trkeperluanMedis (kpm_nama_barang, kpm_jumlah, kpm_satuan, kpm_tanggal_pengajuan, kpm_status, kpm_id_admin, kpm_id_manajer) values(@p1, @p2, @p3, @p4, @p5, @p6, @p7)";
                    SqlCommand command = new SqlCommand(query, _connection);
                    command.Parameters.AddWithValue("@p1", keperluanMedis.kpm_nama_barang);
                    command.Parameters.AddWithValue("@p2", keperluanMedis.kpm_jumlah);
                    command.Parameters.AddWithValue("@p3", keperluanMedis.kpm_satuan);
                    command.Parameters.AddWithValue("@p4", keperluanMedis.kpm_tanggal_pengajuan);
                    command.Parameters.AddWithValue("@p5", keperluanMedis.kpm_status);
                    command.Parameters.AddWithValue("@p6", keperluanMedis.kpm_id_admin);
                    command.Parameters.AddWithValue("@p7", keperluanMedis.kpm_id_manajer);
                    _connection.Open();
                    command.ExecuteNonQuery();
                    _connection.Close();
                }
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

        public void acceptData(int id, string id_manajer)
        {
            try
            {
                string query = "update ahl_trkeperluanMedis set kpm_status='diterima', kpm_tanggal_aksi=@p2, kpm_id_manajer=@p3 where kpm_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", @DateTime.Now);
                command.Parameters.AddWithValue("@p3", id_manajer);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void rejectData(int id, string id_manajer)
        {
            try
            {
                string query = "update ahl_trkeperluanMedis set kpm_status='ditolak', kpm_tanggal_aksi=@p2, kpm_id_manajer=@p3 where kpm_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", @DateTime.Now);
                command.Parameters.AddWithValue("@p3", id_manajer);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void recieveData(int id)
        {
            try
            {
                string query = "update ahl_trkeperluanMedis set kpm_status='barang diterima', kpm_tanggal_diterima=@p2 where kpm_id = @p1";
                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", @DateTime.Now);
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
