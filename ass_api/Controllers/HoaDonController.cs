using ass_api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ass_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HoaDonController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("viewOrder")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Người dùng chưa đăng nhập!" });
                }
                var orders = await _context.HoaDons
                    .Where(h => h.KhachHangID == userId)
                    .Include(h => h.HoaDonDetail)
                    .ThenInclude(d => d.Product) 
                    .Select(h => new
                    {
                        HoaDonID = h.HoaDonID,
                        NgayTao = h.NgayTao,
                        TongTien = h.TongTien,
                        TrangThai = h.TrangThai,
                        SoDienThoai = h.SoDienThoai,
                        GhiChu = h.GhiChu,
                        ChiTiet = h.HoaDonDetail.Select(ct => new
                        {
                            SanPhamID = ct.SanPhamID,
                            TenSanPham = ct.Product.Name,
                            HinhAnh = ct.Product.HinhAnh,
                            Gia = ct.Gia,
                            SoLuong = ct.SoLuong,
                            ThanhTien = ct.SoLuong * ct.Gia
                        }).ToList()
                    })
                    .ToListAsync();

                if (!orders.Any())
                {
                    return NotFound(new { message = "Không có đơn hàng nào." });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server", error = ex.Message });
            }
        }
        
        [HttpPost("cancelOrder/{orderId}")]
        public async Task<IActionResult> HuyDon(int orderId)
        {
            try
            {
                var hoaDon = _context.HoaDons.FirstOrDefault(h => h.HoaDonID == orderId);
                var chiTietHoaDon = _context.HoaDonDetails.Where(ct => ct.HoaDonID == orderId).ToList();

                if (hoaDon == null)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng!" });
                }

                if (hoaDon.TrangThai == "Đã xác nhận" || hoaDon.TrangThai == "Đang Giao Hàng")
                {
                    return BadRequest(new { message = "Không thể hủy đơn hàng đã xác nhận hoặc đang giao!" });
                }
                _context.HoaDonDetails.RemoveRange(chiTietHoaDon);
                _context.HoaDons.Remove(hoaDon);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Đơn hàng đã được hủy và xóa thành công!" });


            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi hủy đơn hàng!", error = ex.Message });
            }
        }
        [Authorize(Roles ="admin")]
        [HttpGet("adminViewOrder")]
        public async Task<IActionResult> GetAdminOrder()
        {
            var hoaDons = await _context.HoaDons
                .Include(hd => hd.User) 
                .Include(hd => hd.HoaDonDetail)
                .ThenInclude(ct => ct.Product)
                .Select(hd => new
                {
                    HoaDonID = hd.HoaDonID,
                    FullName = hd.User.FullName, 
                    NgayTao = hd.NgayTao,
                    TongTien = hd.TongTien,
                    TrangThai = hd.TrangThai,
                    SoDienThoai = hd.SoDienThoai,
                    GhiChu = hd.GhiChu,
                    ChiTiet = hd.HoaDonDetail.Select(ct => new
                    {
                        SanPhamID = ct.SanPhamID,
                        TenSanPham = ct.Product.Name,
                        HinhAnh = ct.Product.HinhAnh,
                        Gia = ct.Gia,
                        SoLuong = ct.SoLuong,
                        ThanhTien = ct.SoLuong * ct.Gia
                    }).ToList()
                })
                .ToListAsync();

            return Ok(hoaDons);
        }


        [Authorize(Roles = "admin")]
        [HttpGet("adminViewOrderFiltered")]
        public async Task<IActionResult> GetAdminOrderFiltered(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.HoaDons
                .Include(hd => hd.User)
                .Include(hd => hd.HoaDonDetail)
                .ThenInclude(ct => ct.Product)
                .AsQueryable();
            if (startDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTao >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(hd => hd.NgayTao <= endDate.Value);
            }

            var hoaDons = await query
                .Select(hd => new
                {
                    HoaDonID = hd.HoaDonID,
                    FullName = hd.User.FullName,
                    NgayTao = hd.NgayTao,
                    TongTien = hd.TongTien,
                    TrangThai = hd.TrangThai,
                    SoDienThoai = hd.SoDienThoai,
                    GhiChu = hd.GhiChu,
                    ChiTiet = hd.HoaDonDetail.Select(ct => new
                    {
                        SanPhamID = ct.SanPhamID,
                        TenSanPham = ct.Product.Name,
                        HinhAnh = ct.Product.HinhAnh,
                        Gia = ct.Gia,
                        SoLuong = ct.SoLuong,
                        ThanhTien = ct.SoLuong * ct.Gia
                    }).ToList()
                })
                .ToListAsync();

            return Ok(hoaDons);
        }



        [Authorize(Roles ="admin")]
        [HttpPost("confirmDelivery/{id}")]
        public IActionResult ConfirmDelivery(int id)
        {
            var hoaDon = _context.HoaDons.Find(id);
            if (hoaDon == null)
            {
                return NotFound(new { message = "Không tìm thấy hóa đơn." });
            }

            hoaDon.TrangThai = "Đang Giao Hàng";
            _context.SaveChanges();

            return Ok(new { message = "Hóa đơn đã được cập nhật." });
        }


    }
}
