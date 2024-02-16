using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AstraHealth.Models
{
    public class AkunModel
    {
        [Required(ErrorMessage = "ID wajib diisi.")]
        [MaxLength(50, ErrorMessage = "ID maksimal 20 karakter.")]
        public string akn_id { get; set; }
        [Required(ErrorMessage = "Nama wajib diisi.")]
        [MaxLength(50, ErrorMessage = "Nama maksimal 50 karakter.")]
        public string? akn_nama { get; set; }
        [Required(ErrorMessage = "Password wajib diisi.")]
        [MaxLength(50, ErrorMessage = "Password maksimal 20 karakter.")]
        public string akn_password { get; set; }
        [Required(ErrorMessage = "Role wajib dipilih.")]
        public string? akn_role { get; set; }
        public string? akn_status { get; set; }
        public string? akn_password_baru { get; set; }
    }
    public class ValidateOldPasswordRequest
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }
    }
}
