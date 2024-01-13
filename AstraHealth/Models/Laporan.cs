using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AstraHealth.Models
{
    public class Laporan
    {
        private readonly string _connectionString;
        private readonly SqlConnection _connection;
        public Laporan(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(_connectionString);
        }

        public List<LaporanModel> laporanDiagnosaSakit(string dari, string sampai)
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();

            try
            {
                if (dari == null && sampai == null)
                {
                    // Mendapatkan tanggal pertama bulan ini
                    DateTime tanggalPertama = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dari = tanggalPertama.ToString("yyyy-MM-dd");

                    // Mendapatkan tanggal terakhir bulan ini
                    DateTime tanggalTerakhir = tanggalPertama.AddMonths(1).AddDays(-1);
                    sampai = tanggalTerakhir.ToString("yyyy-MM-dd");
                }

                // Modifikasi query SQL untuk memfilter berdasarkan tanggal
                string query = "SELECT * from ahl_tranamnesa WHERE anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir ORDER BY anm_diagnosa ASC";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        anm_id_pasien = reader["anm_id_pasien"].ToString(),
                        anm_nama_pasien = reader["anm_nama_pasien"].ToString(),
                        anm_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        anm_keluhan = reader["anm_keluhan"].ToString(),
                        anm_tensi = reader["anm_tensi"].ToString(),
                        anm_diagnosa = reader["anm_diagnosa"].ToString(),
                        tanggal = reader.GetDateTime(reader.GetOrdinal("anm_tanggal")).ToString("dd/MM/yyyy HH:mm:ss")
                    };

                    laporanList.Add(laporan);
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

            return laporanList;
        }


        public List<LaporanModel> laporanProdiDanDepartemen()
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();

            try
            {
                string query = "SELECT anm_id_pasien, anm_nama_pasien, anm_prodi_atau_departemen, anm_keluhan, anm_tensi, anm_diagnosa, anm_tanggal FROM ahl_tranamnesa";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        anm_id_pasien = reader["anm_id_pasien"].ToString(),
                        anm_nama_pasien = reader["anm_nama_pasien"].ToString(),
                        anm_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        anm_keluhan = reader["anm_keluhan"].ToString(),
                        anm_tensi = reader["anm_tensi"].ToString(),
                        anm_diagnosa = reader["anm_diagnosa"].ToString(),
                        anm_tanggal = reader.GetDateTime(reader.GetOrdinal("anm_tanggal"))
                    };

                    laporanList.Add(laporan);
                }
                reader.Close();
                _connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return laporanList;
        }

        public List<PemakaianObatModel> laporanPemaiakaianObat()
        {
            List<PemakaianObatModel> pemakaianObatList = new List<PemakaianObatModel>();
            try
            {
                string query = "SELECT po.pmo_id, po.pmo_id_anamnesa, po.pmo_nama_obat, po.pmo_jumlah, po.pmo_satuan, anm.anm_tanggal " +
                               "FROM ahl_trpemakaianObat po " +
                               "JOIN ahl_tranamnesa anm ON po.pmo_id_anamnesa = anm.anm_id";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PemakaianObatModel obat = new PemakaianObatModel
                    {
                        pmo_id = reader["pmo_id"].ToString(),
                        pmo_id_anamnesa = reader["pmo_id_anamnesa"].ToString(),
                        pmo_nama_obat = reader["pmo_nama_obat"].ToString(),
                        pmo_jumlah = Convert.ToInt32(reader["pmo_jumlah"]),
                        pmo_satuan = reader["pmo_satuan"].ToString(),
                        pmo_tanggal = Convert.ToDateTime(reader["anm_tanggal"]),
                    };

                    pemakaianObatList.Add(obat);
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

            return pemakaianObatList;
        }

        public List<RujukanPasienModel> laporanKecelakaanKerjaDanRujukan()
        {
            List<RujukanPasienModel> laporanList = new List<RujukanPasienModel>();

            try
            {
                // Query pertama untuk mengambil data rujukan dan join dengan anamnesa
                string queryRujukan = "SELECT r.rjk_id, r.rjk_id_anamnesa, a.anm_id_pasien, a.anm_nama_pasien, " +
                                      "a.anm_prodi_atau_departemen, a.anm_diagnosa, a.anm_kecelakaan_kerja, " +
                                      "r.rjk_rumah_sakit, r.rjk_keterangan, r.rjk_tanggal " +
                                      "FROM ahl_trrujukan r " +
                                      "JOIN ahl_tranamnesa a ON r.rjk_id_anamnesa = a.anm_id";

                SqlCommand commandRujukan = new SqlCommand(queryRujukan, _connection);
                _connection.Open();
                SqlDataReader readerRujukan = commandRujukan.ExecuteReader();
                while (readerRujukan.Read())
                {
                    RujukanPasienModel laporan = new RujukanPasienModel
                    {
                        rjk_id = Convert.ToInt32(readerRujukan["rjk_id"]),
                        rjk_id_anamnesa = readerRujukan["rjk_id_anamnesa"].ToString(),
                        rjk_id_pasien = readerRujukan["anm_id_pasien"].ToString(),
                        rjk_nama_pasien = readerRujukan["anm_nama_pasien"].ToString(),
                        rjk_prodi_atau_departemen = readerRujukan["anm_prodi_atau_departemen"].ToString(),
                        rjk_diagnosa = readerRujukan["anm_diagnosa"].ToString(),
                        rjk_kecelakaan_kerja = Convert.ToInt32(readerRujukan["anm_kecelakaan_kerja"]),
                        rjk_rumah_sakit = readerRujukan["rjk_rumah_sakit"].ToString(),
                        rjk_keterangan = readerRujukan["rjk_keterangan"].ToString(),
                        rjk_tanggal = readerRujukan.GetDateTime(readerRujukan.GetOrdinal("rjk_tanggal"))
                    };

                    laporanList.Add(laporan);
                }
                readerRujukan.Close();

                // Query kedua untuk mengambil data anamnesa dengan kecelakaan kerja = 1 yang belum tergabung dalam hasil query pertama
                string queryAnamnesaKecelakaanKerja = "SELECT * FROM ahl_tranamnesa a " +
                                                      "WHERE a.anm_kecelakaan_kerja = 1 AND NOT EXISTS " +
                                                      "(SELECT 1 FROM ahl_trrujukan r WHERE r.rjk_id_anamnesa = a.anm_id)";

                SqlCommand commandAnamnesaKecelakaanKerja = new SqlCommand(queryAnamnesaKecelakaanKerja, _connection);
                SqlDataReader readerAnamnesaKecelakaanKerja = commandAnamnesaKecelakaanKerja.ExecuteReader();
                while (readerAnamnesaKecelakaanKerja.Read())
                {
                    RujukanPasienModel laporan = new RujukanPasienModel
                    {
                        rjk_id_pasien = readerAnamnesaKecelakaanKerja["anm_id_pasien"].ToString(),
                        rjk_nama_pasien = readerAnamnesaKecelakaanKerja["anm_nama_pasien"].ToString(),
                        rjk_prodi_atau_departemen = readerAnamnesaKecelakaanKerja["anm_prodi_atau_departemen"].ToString(),
                        rjk_diagnosa = readerAnamnesaKecelakaanKerja["anm_diagnosa"].ToString(),
                        rjk_kecelakaan_kerja = Convert.ToInt32(readerAnamnesaKecelakaanKerja["anm_kecelakaan_kerja"]),
                    };

                    laporanList.Add(laporan);
                }
                readerAnamnesaKecelakaanKerja.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _connection.Close();
            }

            return laporanList;
        }

        public List<LaporanModel> getLaporan(string dari, string sampai)
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();
            try
            {
                // Modifikasi query untuk menyesuaikan dengan parameter tanggal
                string query = "SELECT * FROM ahl_tranamnesa ORDER BY anm_tanggal DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        anm_id = reader["anm_id"].ToString(),
                        anm_id_pasien = reader["anm_id_pasien"].ToString(),
                        anm_nama_pasien = reader["anm_nama_pasien"].ToString(),
                        anm_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        anm_diagnosa = reader["anm_diagnosa"].ToString(),
                        anm_kecelakaan_kerja = Convert.ToInt32(reader["anm_kecelakaan_kerja"].ToString()),
                        anm_keterangan = reader["anm_keterangan"].ToString(),
                        anm_tanggal = reader.GetDateTime(reader.GetOrdinal("anm_tanggal")),
                        anm_id_admin = reader["anm_id_admin"].ToString(),
                    };

                    laporanList.Add(laporan);
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


            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> distinctDiagnosa = getDistinctDiagnosa(dari, sampai);
            // Combine the results of both lists
            laporanList.AddRange(distinctDiagnosa);

            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> distinctPemakaianObat = getDistinctPemakaianObat();
            // Combine the results of both lists
            laporanList.AddRange(distinctPemakaianObat);

            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> kecelakaanKerjaRujukan = getKecelakaanKerjaDanRujukan();
            // Combine the results of both lists
            laporanList.AddRange(kecelakaanKerjaRujukan);

            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> distinctProdiDanDepartemen = getDistinctProdiDanDepartemen();
            // Combine the results of both lists
            laporanList.AddRange(distinctProdiDanDepartemen);

            return laporanList;
        }

        public List<LaporanModel> getDistinctDiagnosa(string dari, string sampai)
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();
            try
            {
                if (string.IsNullOrEmpty(dari) && string.IsNullOrEmpty(sampai))
                {
                    // Mendapatkan tanggal pertama bulan ini
                    DateTime tanggalPertama = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    dari = tanggalPertama.ToString("yyyy-MM-dd");

                    // Mendapatkan tanggal terakhir bulan ini
                    DateTime tanggalTerakhir = tanggalPertama.AddMonths(1).AddDays(-1);
                    sampai = tanggalTerakhir.ToString("yyyy-MM-dd");
                }

                string query = "SELECT anm_diagnosa, COUNT(*) as jumlah_diagnosa " +
                               "FROM ahl_tranamnesa " +
                               "WHERE anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir " +
                               "GROUP BY anm_diagnosa " +
                               "ORDER BY jumlah_diagnosa DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        diagnosa_sakit = reader["anm_diagnosa"].ToString(),
                        jumlah_diagnosa = Convert.ToInt32(reader["jumlah_diagnosa"]),
                    };

                    laporanList.Add(laporan);
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
            return laporanList;
        }

        public List<LaporanModel> getDistinctPemakaianObat()
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();
            try
            {
                string query = "SELECT pmo_nama_obat, SUM(pmo_jumlah) AS jumlah_pemakaian_obat " +
                               "FROM ahl_trpemakaianObat " +
                               "GROUP BY pmo_nama_obat " +
                               "ORDER BY jumlah_pemakaian_obat DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        nama_obat = reader["pmo_nama_obat"].ToString(),
                        jumlah_pemakaian_obat = Convert.ToInt32(reader["jumlah_pemakaian_obat"]),
                    };

                    laporanList.Add(laporan);
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
            return laporanList;
        }

        public List<LaporanModel> getKecelakaanKerjaDanRujukan()
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();
            try
            {
                string query = "SELECT " +
                               "(SELECT COUNT(*) FROM ahl_tranamnesa WHERE anm_kecelakaan_kerja = 1) AS jumlah_kecelakaan_kerja, " +
                               "(SELECT COUNT(*) FROM ahl_trrujukan) AS jumlah_rujukan";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        jumlah_kecelakaan_kerja = Convert.ToInt32(reader["jumlah_kecelakaan_kerja"]),
                        jumlah_rujukan = Convert.ToInt32(reader["jumlah_rujukan"]),
                    };

                    laporanList.Add(laporan);
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
            return laporanList;
        }

        public List<LaporanModel> getDistinctProdiDanDepartemen()
        {
            List<LaporanModel> laporanList = new List<LaporanModel>();
            try
            {
                string query = "SELECT anm_prodi_atau_departemen, COUNT(*) as jumlah_prodi_atau_departemen " +
                               "FROM ahl_tranamnesa " +
                               "GROUP BY anm_prodi_atau_departemen " +
                               "ORDER BY jumlah_prodi_atau_departemen DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        nama_prodi_atau_departemen = reader["anm_prodi_atau_departemen"].ToString(),
                        jumlah_prodi_atau_departemen = Convert.ToInt32(reader["jumlah_prodi_atau_departemen"]),
                    };

                    laporanList.Add(laporan);
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
            return laporanList;
        }
    }
}
