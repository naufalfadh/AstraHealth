namespace AstraHealth.Models
{
    public class KeperluanMedisModel
    {
        public int kpm_id {  get; set; }
        public string kpm_nama_barang{ get; set; }
        public int kpm_jumlah { get; set; }
        public string kpm_satuan { get; set; }
        public DateTime kpm_tanggal_pengajuan { get; set; }
        public DateTime? kpm_tanggal_aksi { get; set; }
        public DateTime? kpm_tanggal_diterima { get; set; }
        public string kpm_status { get; set; }
        public string? kpm_catatan { get; set; }
        public string kpm_id_admin { get; set; }
        public string kpm_id_manajer { get; set; }
        public string? kpm_nama_manajer { get; set; }
        public string? kpm_nama_admin { get; set; }
    }

    public class KeperluanAlatMedis
    {
        public List<KeperluanMedisModel> keperluanMedis { get; set; }
    }
}
