using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AstraHealth.Models
{
    public class ObatModel
    {
        public int obt_id { get; set; }
        public string obt_nama_obat { get; set; }
        public int obt_stok { get; set; }
        public string obt_satuan { get; set; }
        public DateTime obt_tanggal_kadaluarsa { get; set; }
        public string obt_status { get; set; }
    }
}
