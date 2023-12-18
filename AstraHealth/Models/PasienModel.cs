using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AstraHealth.Models
{
    public class PasienModel
    {
        public string anm_id { get; set; }
        public string anm_id_pasien { get; set; }
        public string anm_nama_pasien { get; set; }
        public string anm_prodi_atau_departemen { get; set; }
        public string anm_keluhan { get; set; }
        public string? anm_tensi { get; set; }
        public string anm_diagnosa { get; set; }
        public int anm_kecelakaan_kerja { get; set; }
        public string? anm_keterangan { get; set; }
        public DateTime anm_tanggal { get; set; }
        public string anm_id_admin { get; set; }
        public string? anm_nama_admin { get; set; }
        public List<PemakaianObatModel> PemakaianObats { get; set; } = new List<PemakaianObatModel>();
    }

    public class PemakaianObatModel
    {
        public string pmo_id { get; set; }
        public string pmo_id_anamnesa { get; set; }
        public string pmo_nama_obat { get; set; }
        public int pmo_jumlah { get; set; }
        public string pmo_satuan { get; set; }
        public DateTime? pmo_tanggal { get; set; }
    }
}
