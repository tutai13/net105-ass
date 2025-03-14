using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ass_api.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string ChiTiet { get; set; }

        [Required]
        public int Gia { get; set; }

        [Required]
        [StringLength(200)]
        public string HinhAnh { get; set; }

        // Khóa ngoại liên kết với Loai
        [ForeignKey("Loai")]
        public int LoaiID { get; set; }
        [JsonIgnore]
        public virtual Loai? Loai { get; set; }
    }
}
