using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class Pasien
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        public Pasien(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<PasienModel> getAllData()
        {
            List<PasienModel> pasienList = new List<PasienModel>();
            try
            {
                string query = "SELECT t.*, a.akn_nama AS anm_nama_admin FROM ahl_tranamnesa t JOIN ahl_msakun a ON t.anm_id_admin = a.akn_id ORDER BY t.anm_tanggal DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PasienModel pasien = new PasienModel
                    {
                        anm_id = reader["anm_id"].ToString(),
                        anm_id_pasien = reader["anm_id_pasien"].ToString(),
                        anm_nama_pasien = reader["anm_nama_pasien"].ToString(),
                        anm_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        anm_keluhan = reader["anm_keluhan"].ToString(),
                        anm_diagnosa = reader["anm_diagnosa"].ToString(),
                        anm_kecelakaan_kerja = Convert.ToInt32(reader["anm_kecelakaan_kerja"].ToString()),
                        anm_keterangan = reader["anm_keterangan"].ToString(),
                        anm_tanggal = reader.GetDateTime(reader.GetOrdinal("anm_tanggal")),
                        anm_id_admin = reader["anm_id_admin"].ToString(),
                        anm_nama_admin = reader["anm_nama_admin"].ToString(), // Tambahkan properti untuk nama admin
                        PemakaianObats = new List<PemakaianObatModel>()
                    };

                    pasienList.Add(pasien);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            getAllPemakaianObat(pasienList);

            return pasienList;
        }

        public List<PasienModel> getAllPemakaianObat(List<PasienModel> pasienList)
        {
            try
            {
                string query = "SELECT p.*, o.obt_nama_obat, o.obt_satuan " +
                               "FROM ahl_trpemakaianObat p " +
                               "JOIN ahl_msobat o ON p.pmo_id_obat = o.obt_id";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PemakaianObatModel obat = new PemakaianObatModel
                    {
                        pmo_id_anamnesa = reader["pmo_id_anamnesa"].ToString(),
                        pmo_nama_obat = reader["obt_nama_obat"].ToString(),
                        pmo_jumlah = Convert.ToInt32(reader["pmo_jumlah"]),
                        pmo_satuan = reader["obt_satuan"].ToString(),
                    };

                    // Temukan anamnesa yang sesuai dan tambahkan pemakaian obat ke daftar PemakaianObats
                    var pasien = pasienList.Find(p => p.anm_id == obat.pmo_id_anamnesa);
                    if (pasien != null)
                    {
                        pasien.PemakaianObats.Add(obat);
                    }
                }

                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return pasienList;
        }


        public void insertData(PasienModel pasienModel)
        {
            try
            {
                string query = "insert into ahl_tranamnesa values(@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", pasienModel.anm_id);
                command.Parameters.AddWithValue("@p2", pasienModel.anm_id_pasien);
                command.Parameters.AddWithValue("@p3", pasienModel.anm_nama_pasien);
                command.Parameters.AddWithValue("@p4", pasienModel.anm_prodi_atau_departemen);
                command.Parameters.AddWithValue("@p5", pasienModel.anm_keluhan);
                command.Parameters.AddWithValue("@p6", pasienModel.anm_diagnosa);
                command.Parameters.AddWithValue("@p7", pasienModel.anm_kecelakaan_kerja);
                command.Parameters.AddWithValue("@p8", (object)pasienModel.anm_keterangan ?? DBNull.Value);
                command.Parameters.AddWithValue("@p9", pasienModel.anm_tanggal);
                command.Parameters.AddWithValue("@p10", pasienModel.anm_id_admin);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            insertPemakaianObat(pasienModel);
        }

        public void insertPemakaianObat(PasienModel pasienModel)
        {
            try
            {
                foreach (var pemakaianObat in pasienModel.PemakaianObats)
                {
                    string query = "INSERT INTO ahl_trpemakaianObat VALUES(@p1, @p2, @p3)";
                    using (SqlCommand command = new SqlCommand(query, _connection))
                    {
                        command.Parameters.AddWithValue("@p1", pemakaianObat.pmo_id_anamnesa);
                        command.Parameters.AddWithValue("@p2", pemakaianObat.pmo_jumlah);
                        command.Parameters.AddWithValue("@p3", pemakaianObat.pmo_id_obat);

                        _connection.Open();
                        command.ExecuteNonQuery();
                        _connection.Close();
                    }

                    decreaseStokObat(pemakaianObat.pmo_id_obat, pemakaianObat.pmo_jumlah);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void decreaseStokObat(int id, int jumlah)
        {
            try
            {
                string query = "UPDATE ahl_msobat SET obt_stok = obt_stok - @p2 WHERE obt_id = @p1";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@p1", id);
                    command.Parameters.AddWithValue("@p2", jumlah);

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

        public string getAnamnesaId()
        {
            try
            {
                string query = "SELECT COUNT(*) + 1 AS NewId FROM ahl_tranamnesa";
                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    int newId = Convert.ToInt32(result);
                    return newId.ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        /*public void updateData(PasienModel pasienModel)
        {
            try
            {
                string query = "update trregistrasi " +
                "set rgs_id_admin = @p2, " +
                "rgs_id_pasien = @p3, " +
                "rgs_nama_pasien = @p4, " +
                "rgs_prodi_atau_departemen = @p5, " +
                "rgs_keluhan = @p6, " +
                "rgs_diagnosa = @p8, " +
                "rgs_obat = @p9, " +
                "rgs_jumlah_obat = @p10, " +
                "rgs_kecelakaan_kerja = @p11, " +
                "rgs_keterangan = @p12 " +
                "rgs_tanggal = @p13, " +
                "rgs_id_admin = @p14 " +
                "where rgs_id = @p1";

                using SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@p1", pasienModel.rgs_id_admin);
                command.Parameters.AddWithValue("@p2", pasienModel.rgs_id_pasien);
                command.Parameters.AddWithValue("@p3", pasienModel.rgs_nama_pasien);
                command.Parameters.AddWithValue("@p4", pasienModel.rgs_prodi_atau_departemen);
                command.Parameters.AddWithValue("@p5", pasienModel.rgs_keluhan);
                command.Parameters.AddWithValue("@p7", pasienModel.rgs_diagnosa);
                command.Parameters.AddWithValue("@p8", pasienModel.rgs_obat);
                command.Parameters.AddWithValue("@p9", pasienModel.rgs_jumlah_obat);
                command.Parameters.AddWithValue("@p10", pasienModel.rgs_kecelakaan_kerja);
                command.Parameters.AddWithValue("@p11", pasienModel.rgs_keterangan);
                command.Parameters.AddWithValue("@p12", pasienModel.rgs_tanggal);
                command.Parameters.AddWithValue("@p13", pasienModel.rgs_id_admin);
                _connection.Open();
                command.ExecuteNonQuery();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/

        /*public void deleteData(int id)
        {
            try
            {
                string query = "delete from trregistrasi where rgs_id = @p1";
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
        }*/

        public List<ObatModel> getObatData()
        {
            List<ObatModel> obatList = new List<ObatModel>();
            try
            {
                string query = "select * from ahl_msobat WHERE obt_status = 'aktif' AND obt_stok > 0";
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

        public ObatModel getObatDataById(int id)
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

        public void deleteData(int id)
        {
            try
            {
                string queryDeleteAnamnesa = "DELETE FROM ahl_tranamnesa WHERE anm_id = @p1";
                string queryDeletePemakaianObat = "DELETE FROM ahl_trpemakaianObat WHERE pmo_id_anamnesa = @p1";

                string queryRestoreObatStock = @"
                        UPDATE ahl_msobat 
                        SET obt_stok = obt_stok + (
                            SELECT ISNULL(SUM(pmo_jumlah), 0) 
                            FROM ahl_trpemakaianObat 
                            WHERE pmo_id_anamnesa = @p1
                        )
                        WHERE obt_id IN (
                            SELECT pmo_id_obat 
                            FROM ahl_trpemakaianObat 
                            WHERE pmo_id_anamnesa = @p1
                        )";

                using (SqlCommand commandDeleteAnamnesa = new SqlCommand(queryDeleteAnamnesa, _connection))
                using (SqlCommand commandDeletePemakaianObat = new SqlCommand(queryDeletePemakaianObat, _connection))
                using (SqlCommand commandRestoreObatStock = new SqlCommand(queryRestoreObatStock, _connection))
                {
                    commandDeleteAnamnesa.Parameters.AddWithValue("@p1", id);
                    commandDeletePemakaianObat.Parameters.AddWithValue("@p1", id);
                    commandRestoreObatStock.Parameters.AddWithValue("@p1", id);

                    _connection.Open();

                    // Mulai transaksi
                    SqlTransaction transaction = _connection.BeginTransaction();
                    commandDeleteAnamnesa.Transaction = transaction;
                    commandDeletePemakaianObat.Transaction = transaction;
                    commandRestoreObatStock.Transaction = transaction;

                    try
                    {
                        // Restore stok obat terlebih dahulu
                        commandRestoreObatStock.ExecuteNonQuery();

                        // Kemudian hapus data pemakaian obat dan anamnesa
                        commandDeletePemakaianObat.ExecuteNonQuery();
                        commandDeleteAnamnesa.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        _connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



    }
}
