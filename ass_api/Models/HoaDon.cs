using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ass_api.Models
{
    [Table("HoaDons")]
    public class HoaDon
    {
        public int HoaDonID { get; set; }
        [ForeignKey("User")]
        public string KhachHangID { get; set; }
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public int TongTien { get; set; }
        public string TrangThai { get; set; } = "Đang Xử Lý";
        public string SoDienThoai { get; set; }
        public string? GhiChu { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; }
        [JsonIgnore]

        public virtual ICollection<HoaDonDetail> HoaDonDetail { get; set; }
    }
}
