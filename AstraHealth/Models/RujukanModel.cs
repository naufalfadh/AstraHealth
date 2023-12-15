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
    }
}
