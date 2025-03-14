using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ass_api.Models
{
    public class Loai
    {
        [Key]
        public int LoaiID { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; }

       
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
