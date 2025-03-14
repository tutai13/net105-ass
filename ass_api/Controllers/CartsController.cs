using ass_api.Data;
using ass_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace ass_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Getcarts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var query = from cart in _context.Carts
                        join product in _context.Products
                        on cart.ProductID equals product.ProductID
                        where cart.UserId == userId
                        select new
                        {
                            cartID = cart.CartID,
                            userId = cart.UserId,
                            productID = cart.ProductID,
                            soLuong = cart.SoLuong,
                            hinhAnh = product.HinhAnh,
                            gia = product.Gia,
                            tenSanPham = product.Name 
                        };

            var result = await query.ToListAsync();
            return Ok(result);
        }
        
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            if (request == null || request.ProductID <= 0 || request.SoLuong <= 0)
                return BadRequest(new { message = "Dữ liệu không hợp lệ" });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var product = await _context.Products.FindAsync(request.ProductID);
            if (product == null)
                return NotFound(new { message = "Sản phẩm không tồn tại" });

            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductID == request.ProductID);

            if (existingCartItem != null)
            {
                existingCartItem.SoLuong += request.SoLuong;
            }
            else
            {
                var cartItem = new Cart
                {
                    UserId = userId,
                    ProductID = request.ProductID,
                    SoLuong = request.SoLuong
                };
                _context.Carts.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Sản phẩm đã được thêm vào giỏ hàng" });
        }
        public class CartRequest
        {
            public int ProductID { get; set; }
            public int SoLuong { get; set; }
        }


        [HttpGet("order")]
        public async Task<IActionResult> GetOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderDetails = await _context.Carts
                .Where(cart => cart.UserId == userId)
                .Join(
                    _context.Users,
                    cart => cart.UserId,
                    user => user.Id,
                    (cart, user) => new { CartItem = cart, UserInfo = user }
                )
                .Join(
                    _context.Products,
                    cartAndUser => cartAndUser.CartItem.ProductID,
                    product => product.ProductID,
                    (cartAndUser, product) => new
                    {
                        FullName = cartAndUser.UserInfo.FullName,
                        Email = cartAndUser.UserInfo.Email,
                        PhoneNumber = cartAndUser.UserInfo.PhoneNumber,
                        ProductName = product.Name,
                        Gia = product.Gia,
                        HinhAnh = product.HinhAnh,
                        SoLuong = cartAndUser.CartItem.SoLuong,
                        TotalPrice = cartAndUser.CartItem.SoLuong * product.Gia
                    }
                )
                .ToListAsync();

            return Ok(orderDetails);
        }

        [HttpPost("pay")]
        public async Task<IActionResult> AddPay([FromBody] PayRequest model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var cartItems = await _context.Carts
                    .Where(item => item.UserId == userId)
                    .Include(item => item.Product)
                    .ToListAsync();

                if (cartItems == null || !cartItems.Any())
                {
                    return NotFound(new { message = "Giỏ hàng rỗng, không thể thanh toán." });
                }
                int totalAmount = cartItems.Sum(item => item.SoLuong * item.Product.Gia);
                var newInvoice = new HoaDon
                {
                    KhachHangID = userId,
                    NgayTao = DateTime.Now,
                    TongTien = totalAmount,
                    TrangThai = "Đang Xử Lý",  
                    SoDienThoai = model.phoneNumber,
                    GhiChu = model.GhiChu
                };

                _context.HoaDons.Add(newInvoice);
                await _context.SaveChangesAsync();
                var invoiceDetails = cartItems.Select(cartItem => new HoaDonDetail
                {
                    HoaDonID = newInvoice.HoaDonID,  
                    SanPhamID = cartItem.ProductID,
                    SoLuong = cartItem.SoLuong,
                    Gia = cartItem.Product.Gia
                }).ToList();

                _context.HoaDonDetails.AddRange(invoiceDetails);
                await _context.SaveChangesAsync();
                _context.Carts.RemoveRange(cartItems);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Thanh toán thành công", invoiceId = newInvoice.HoaDonID });
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Lỗi CSDL: " + ex.InnerException?.Message);
                return StatusCode(500, new { message = "Lỗi khi cập nhật dữ liệu", error = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi Server", error = ex.Message });
            }
        }



        public class PayRequest
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string phoneNumber { get; set; }
            public string? GhiChu { get; set; }
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSoLuong(int cartID, int soLuong)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { success = false, message = "Người dùng không hợp lệ." });
                }
                var gioHang = await _context.Carts
                    .FirstOrDefaultAsync(g => g.CartID == cartID && g.UserId == userId);

                if (gioHang == null)
                {
                    return NotFound(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng." });
                }

                if (soLuong <= 0)
                {
                    return BadRequest(new { success = false, message = "Số lượng phải lớn hơn 0." });
                }

                gioHang.SoLuong = soLuong;
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Cập nhật số lượng thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }
        [HttpDelete("remove/{cartID}")]
        public async Task<IActionResult> RemoveFromCart(int cartID)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { success = false, message = "Người dùng không hợp lệ." });
            }

            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.CartID == cartID && c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng." });
            }

            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
        }
        


    }
}
