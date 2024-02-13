using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AstraHealth.Models
{
    public class LaporanModel
    {
        public string anm_id { get; set; }
        public string anm_id_pasien { get; set; }
        public string anm_nama_pasien { get; set; }
        public string anm_prodi_atau_departemen { get; set; }
        public string anm_keluhan { get; set; }
        public string anm_diagnosa { get; set; }
        public int anm_kecelakaan_kerja { get; set; }
        public string? anm_keterangan { get; set; }
        public DateTime anm_tanggal { get; set; }
        public string tanggal { get; set; }
        public string anm_id_admin { get; set; }
        public string? anm_nama_admin { get; set; }
        public List<PemakaianObatModel> PemakaianObats { get; set; } = new List<PemakaianObatModel>();

        public string? diagnosa_sakit { get; set; }
        public int? jumlah_diagnosa { get; set; }

        public string? nama_obat { get; set; }
        public int? jumlah_pemakaian_obat { get; set; }

        public int? jumlah_kecelakaan_kerja { get; set; }
        public int? jumlah_rujukan { get; set; }

        public string? nama_prodi_atau_departemen { get; set; }
        public int? jumlah_prodi_atau_departemen { get; set; }
    }

    public class DateRangeModel
    {
        public DateTime Dari { get; set; }
        public DateTime Sampai { get; set; }
    }
}