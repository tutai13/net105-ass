using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ass_api.Models
{
    [Table("HoaDonDetails")]
    public class HoaDonDetail
    {
        public int HoaDonDetailID { get; set; }
        [ForeignKey("HoaDon")]
        public int HoaDonID { get; set; }
        [ForeignKey("Product")]
        public int SanPhamID { get; set; }
        public int SoLuong { get; set; }
        public int Gia { get; set; }
        [JsonIgnore]
        public virtual HoaDon HoaDon { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
