namespace AstraHealth.Models
{
    public class KeperluanObatModel
    {
        public int kpo_id {  get; set; }
        public string kpo_nama_obat { get; set; }
        public int kpo_jumlah { get; set; }
        public DateTime kpo_tanggal_pengajuan { get; set; }
        public DateTime kpo_tanggal_aksi { get; set; }
        public string kpo_status { get; set; }
        public string kpo_catatan { get; set; }
        public string kpo_id_admin { get; set; }
    }
}
