using System.ComponentModel.DataAnnotations;


namespace AstraHealth.Models
{
    public class PasienModel
    {
        public int rgs_id { get; set; }
        public string rgs_id_pasien { get; set; }
        public string rgs_nama_pasien { get; set; }
        public string rgs_prodi_atau_departemen { get; set; }
        public string rgs_keluhan { get; set; }
        public string rgs_tensi { get; set; }
        public string rgs_diagnosa { get; set; }
        public string rgs_obat { get; set; }
        public int rgs_jumlah_obat { get; set; }
        public int rgs_kecelakaan_kerja { get; set; }
        public string rgs_keterangan { get; set; }
        public DateTime rgs_tanggal { get; set; }
        public string rgs_id_admin { get; set; }

    }
}
