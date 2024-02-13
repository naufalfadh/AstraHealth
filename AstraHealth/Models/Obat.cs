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
                string query = "select * from ahl_msobat WHERE obt_status = 'aktif' order by obt_nama_obat asc";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ObatModel obat = new ObatModel
                    {
                        obt_id = Convert.ToInt32(reader["obt_id"]),
                        obt_nama_obat = reader["obt_nama_obat"].ToString(),
                        obt_stok = Convert.ToInt32(reader["obt_stok"]),
                        obt_satuan = reader["obt_satuan"].ToString(),
                        obt_tanggal_kadaluarsa = Convert.ToDateTime(reader["obt_tanggal_kadaluarsa"]),
                        obt_status = reader["obt_status"].ToString(),
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
                string query = "select * from ahl_msobat where obt_id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                obatModel.obt_id = Convert.ToInt32(reader["obt_id"]);
                obatModel.obt_nama_obat = reader["obt_nama_obat"].ToString();
                obatModel.obt_stok = Convert.ToInt32(reader["obt_stok"]);
                obatModel.obt_satuan = reader["obt_satuan"].ToString();
                obatModel.obt_tanggal_kadaluarsa = Convert.ToDateTime(reader["obt_tanggal_kadaluarsa"]);
                obatModel.obt_status = reader["obt_status"].ToString();
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obatModel;
        }

        public ObatModel getDataByNama(string nama)
        {
            ObatModel obatModel = null; // Initialize as null
            try
            {
                string query = "select * from ahl_msobat where obt_nama_obat = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", nama);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Periksa apakah ada data yang tersedia
                if (reader.HasRows)
                {
                    obatModel = new ObatModel(); // Instantiate when data is found
                    reader.Read();
                    obatModel.obt_id = Convert.ToInt32(reader["obt_id"]);
                    obatModel.obt_nama_obat = reader["obt_nama_obat"].ToString();
                    obatModel.obt_stok = Convert.ToInt32(reader["obt_stok"]);
                    obatModel.obt_satuan = reader["obt_satuan"].ToString();
                    obatModel.obt_tanggal_kadaluarsa = Convert.ToDateTime(reader["obt_tanggal_kadaluarsa"].ToString());
                    obatModel.obt_status = reader["obt_status"].ToString();
                }
                else
                {
                    Console.WriteLine("Tidak ada data yang ditemukan.");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return obatModel;
        }

        public void insertData(ObatModel obatModel)
        {
            try
            {
                string query = "insert into ahl_msobat values(@p1, @p2, @p3, @p4, @p5)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", obatModel.obt_nama_obat);
                command.Parameters.AddWithValue("@p2", obatModel.obt_stok);
                command.Parameters.AddWithValue("@p3", obatModel.obt_satuan);
                command.Parameters.AddWithValue("@p4", obatModel.obt_status);
                command.Parameters.AddWithValue("@p5", obatModel.obt_tanggal_kadaluarsa);
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
                string query = "update ahl_msobat " +
                "set obt_nama_obat = @p2, " +
                "obt_stok = @p3, " +
                "obt_tanggal_kadaluarsa = @p4, " +
                "obt_satuan = @p5 " +
                "where obt_id = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", obatModel.obt_id);
                command.Parameters.AddWithValue("@p2", obatModel.obt_nama_obat);
                command.Parameters.AddWithValue("@p3", obatModel.obt_stok);
                command.Parameters.AddWithValue("@p4", obatModel.obt_tanggal_kadaluarsa);
                command.Parameters.AddWithValue("@p5", obatModel.obt_satuan);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void updateDataByNama(ObatModel obatModel)
        {
            try
            {
                string query = "UPDATE ahl_msobat " +
                               "SET obt_stok = @p2, " +
                               "obt_tanggal_kadaluarsa = @p3, " +
                               "obt_satuan = @p4, " +
                               "obt_status = @p5 " +
                               "WHERE obt_nama_obat = @p1";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p1", obatModel.obt_nama_obat);
                    command.Parameters.AddWithValue("@p2", obatModel.obt_stok);
                    command.Parameters.AddWithValue("@p3", obatModel.obt_tanggal_kadaluarsa);
                    command.Parameters.AddWithValue("@p4", obatModel.obt_satuan);
                    command.Parameters.AddWithValue("@p5", obatModel.obt_status);

                    _connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public void deleteData(int id)
        {
            try
            {
                string query = "update ahl_msobat set obt_status = 'tidak aktif' where obt_id = @p1";
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
