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

        public List<LaporanModel> laporanProdiDanDepartemen(string dari, string sampai)
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
                string query = "SELECT anm_id_pasien, anm_nama_pasien, anm_prodi_atau_departemen, anm_keluhan, anm_diagnosa, anm_tanggal FROM ahl_tranamnesa WHERE anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir ORDER BY anm_prodi_atau_departemen ASC";

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

        public List<PemakaianObatModel> laporanPemakaianObat(string dari, string sampai)
        {
            List<PemakaianObatModel> pemakaianObatList = new List<PemakaianObatModel>();
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

                // Modifikasi query SQL untuk memfilter berdasarkan tanggal dan melakukan join dengan ahl_msobat
                string query = "SELECT po.pmo_id_anamnesa, mo.obt_nama_obat AS pmo_nama_obat, po.pmo_jumlah, mo.obt_satuan AS pmo_satuan, anm.anm_tanggal " +
                               "FROM ahl_trpemakaianObat po " +
                               "JOIN ahl_tranamnesa anm ON po.pmo_id_anamnesa = anm.anm_id " +
                               "JOIN ahl_msobat mo ON po.pmo_id_obat = mo.obt_id " +
                               "WHERE anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir ORDER BY pmo_nama_obat ASC";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PemakaianObatModel obat = new PemakaianObatModel
                    {
                        pmo_id_anamnesa = reader["pmo_id_anamnesa"].ToString(),
                        pmo_nama_obat = reader["pmo_nama_obat"].ToString(),
                        pmo_jumlah = Convert.ToInt32(reader["pmo_jumlah"]),
                        pmo_satuan = reader["pmo_satuan"].ToString(),
                        tanggal = reader.GetDateTime(reader.GetOrdinal("anm_tanggal")).ToString("dd/MM/yyyy HH:mm:ss")
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

        public List<RujukanPasienModel> laporanKecelakaanKerjaDanRujukan(string dari, string sampai)
        {
            List<RujukanPasienModel> laporanList = new List<RujukanPasienModel>();

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

                // Query untuk mengambil data rujukan dan join dengan anamnesa
                string queryRujukan = "SELECT r.rjk_id, r.rjk_id_anamnesa, a.anm_id_pasien, a.anm_nama_pasien, " +
                                      "a.anm_prodi_atau_departemen, a.anm_diagnosa, a.anm_kecelakaan_kerja, " +
                                      "r.rjk_rumah_sakit, r.rjk_keterangan, r.rjk_tanggal " +
                                      "FROM ahl_trrujukan r " +
                                      "JOIN ahl_tranamnesa a ON r.rjk_id_anamnesa = a.anm_id " +
                                      "WHERE r.rjk_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir";

                SqlCommand commandRujukan = new SqlCommand(queryRujukan, _connection);
                commandRujukan.Parameters.AddWithValue("@TanggalPertama", dari);
                commandRujukan.Parameters.AddWithValue("@TanggalTerakhir", sampai);
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
                        tanggal = readerRujukan.GetDateTime(readerRujukan.GetOrdinal("rjk_tanggal")).ToString("dd/MM/yyyy HH:mm:ss")
                    };

                    laporanList.Add(laporan);
                }
                readerRujukan.Close();

                // Query untuk mengambil data anamnesa dengan kecelakaan kerja = 1 yang belum termasuk dalam hasil query pertama
                string queryAnamnesaKecelakaanKerja = "SELECT * FROM ahl_tranamnesa a " +
                                                      "WHERE a.anm_kecelakaan_kerja = 1 AND NOT EXISTS " +
                                                      "(SELECT 1 FROM ahl_trrujukan r " +
                                                      "WHERE r.rjk_id_anamnesa = a.anm_id AND r.rjk_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir) " +
                                                      "AND a.anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir";

                SqlCommand commandAnamnesaKecelakaanKerja = new SqlCommand(queryAnamnesaKecelakaanKerja, _connection);
                commandAnamnesaKecelakaanKerja.Parameters.AddWithValue("@TanggalPertama", dari);
                commandAnamnesaKecelakaanKerja.Parameters.AddWithValue("@TanggalTerakhir", sampai);
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
                        rjk_keterangan = readerAnamnesaKecelakaanKerja["anm_keterangan"].ToString(),
                        tanggal = readerAnamnesaKecelakaanKerja.GetDateTime(readerAnamnesaKecelakaanKerja.GetOrdinal("anm_tanggal")).ToString("dd/MM/yyyy HH:mm:ss")
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
            List<LaporanModel> distinctPemakaianObat = getDistinctPemakaianObat(dari, sampai);
            // Combine the results of both lists
            laporanList.AddRange(distinctPemakaianObat);

            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> kecelakaanKerjaRujukan = getKecelakaanKerjaDanRujukan(dari, sampai);
            // Combine the results of both lists
            laporanList.AddRange(kecelakaanKerjaRujukan);

            // Panggil method untuk menghitung jumlah diagnosa dan menyimpannya di dalam list yang sama
            List<LaporanModel> distinctProdiDanDepartemen = getDistinctProdiDanDepartemen(dari, sampai);
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

        public List<LaporanModel> getDistinctPemakaianObat(string dari, string sampai)
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

                // Modifikasi query SQL untuk memfilter berdasarkan tanggal dan melakukan join dengan ahl_msobat
                string query = "SELECT mo.obt_nama_obat AS nama_obat, SUM(po.pmo_jumlah) AS jumlah_pemakaian_obat " +
                               "FROM ahl_trpemakaianObat po " +
                               "JOIN ahl_tranamnesa anm ON po.pmo_id_anamnesa = anm.anm_id " +
                               "JOIN ahl_msobat mo ON po.pmo_id_obat = mo.obt_id " +
                               "WHERE anm.anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir " +
                               "GROUP BY mo.obt_nama_obat " +
                               "ORDER BY jumlah_pemakaian_obat DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LaporanModel laporan = new LaporanModel
                    {
                        nama_obat = reader["nama_obat"].ToString(),
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

        public List<LaporanModel> getKecelakaanKerjaDanRujukan(string dari, string sampai)
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
                string query = "SELECT " +
                               "(SELECT COUNT(*) FROM ahl_tranamnesa WHERE anm_kecelakaan_kerja = 1 AND anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir) AS jumlah_kecelakaan_kerja, " +
                               "(SELECT COUNT(*) FROM ahl_trrujukan WHERE rjk_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir) AS jumlah_rujukan";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime
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

        public List<LaporanModel> getDistinctProdiDanDepartemen(string dari, string sampai)
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
                string query = "SELECT anm_prodi_atau_departemen, COUNT(*) as jumlah_prodi_atau_departemen " +
                               "FROM ahl_tranamnesa " +
                               "WHERE anm_tanggal BETWEEN @TanggalPertama AND @TanggalTerakhir " +
                               "GROUP BY anm_prodi_atau_departemen " +
                               "ORDER BY jumlah_prodi_atau_departemen DESC";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@TanggalPertama", dari); // Menggunakan .Value untuk mengakses tipe data DateTime
                command.Parameters.AddWithValue("@TanggalTerakhir", sampai); // Menggunakan .Value untuk mengakses tipe data DateTime
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
