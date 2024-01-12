using System.ComponentModel.DataAnnotations;

namespace AstraHealth.Models
{
    public class RujukanModel
    {
        public int rjk_id { get; set; }
        public string rjk_id_anamnesa { get; set; }
        public string rjk_rumah_sakit { get; set; }
        public string? rjk_keterangan { get; set; }
        public DateTime rjk_tanggal { get; set; }
        public string? anm_id_pasien { get; set; }
        public string? anm_nama_pasien { get; set; }
        public string? anm_prodi_atau_departemen { get; set; }
        public string? anm_diagnosa { get; set; }
    }

    public class RujukanPasienModel
    {
        public int? rjk_id { get; set; }
        public string? rjk_id_anamnesa { get; set; }
        public string? rjk_id_pasien { get; set; }
        public string? rjk_nama_pasien { get; set; }
        public string? rjk_prodi_atau_departemen { get; set; }
        public string? rjk_diagnosa { get; set; }
        public int? rjk_kecelakaan_kerja { get; set; }
        public string? rjk_rumah_sakit { get; set; }
        public string? rjk_keterangan { get; set; }
        public DateTime? rjk_tanggal { get; set; }
    }
}
