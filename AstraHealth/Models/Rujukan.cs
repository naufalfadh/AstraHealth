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
                        rjk_id_anamnesa = reader["rjk_id_anamnesa"].ToString(),
                        rjk_rumah_sakit = reader["rjk_rumah_sakit"].ToString(),
                        rjk_keterangan = reader["rjk_keterangan"].ToString(),
                        rjk_tanggal = Convert.ToDateTime(reader["rjk_tanggal"]),
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

        public RujukanModel getData(int id)
        {
            RujukanModel rujukanModel = new RujukanModel();
            try
            {
                string query = "SELECT * FROM ahl_trrujukan WHERE rjk_id = @p1";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    rujukanModel.rjk_id = Convert.ToInt32(reader["rjk_id"]);
                    rujukanModel.rjk_id_anamnesa = reader["rjk_id_anamnesa"].ToString();
                    rujukanModel.rjk_rumah_sakit = reader["rjk_rumah_sakit"].ToString();
                    rujukanModel.rjk_keterangan = reader["rjk_keterangan"].ToString();
                    rujukanModel.rjk_tanggal = Convert.ToDateTime(reader["rjk_tanggal"]);
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
            return rujukanModel;
        }

        public RujukanModel getAnamnesaData(string id)
        {
            RujukanModel rujukanList = new RujukanModel();
            try
            {
                string query = "SELECT * from ahl_tranamnesa where anm_id = @p1";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", id);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    RujukanModel pasien = new RujukanModel
                    {
                        anm_id_pasien = reader["anm_id_pasien"].ToString(),
                        anm_nama_pasien = reader["anm_nama_pasien"].ToString(),
                        anm_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        anm_diagnosa = reader["anm_diagnosa"].ToString(),
                    };

                    rujukanList = pasien;
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

        public void insertData(RujukanModel rujukanModel)
        {
            try
            {
                string query = "INSERT INTO [DB_ASTRAhealth].[dbo].[ahl_trrujukan] ([rjk_id_anamnesa], [rjk_rumah_sakit], [rjk_keterangan], [rjk_tanggal]) VALUES (@p1, @p2, @p3, @p4)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", rujukanModel.rjk_id_anamnesa);
                command.Parameters.AddWithValue("@p2", rujukanModel.rjk_rumah_sakit);
                command.Parameters.AddWithValue("@p3", (object)rujukanModel.rjk_keterangan ?? DBNull.Value);
                command.Parameters.AddWithValue("@p4", rujukanModel.rjk_tanggal);
                _connection.Open();
                command.ExecuteNonQuery();
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

        public void updateData(RujukanModel rujukanModel)
        {
            try
            {
                string query = "UPDATE [DB_ASTRAhealth].[dbo].[ahl_trrujukan] " +
                               "SET [rjk_keterangan] = @p2 " +
                               "WHERE [rjk_id] = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", rujukanModel.rjk_id);
                command.Parameters.AddWithValue("@p2", rujukanModel.rjk_keterangan);
                _connection.Open();
                command.ExecuteNonQuery();
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



    }
}
