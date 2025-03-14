using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ass_api.Data;
using ass_api.Models;
using Microsoft.AspNetCore.Authorization;

namespace ass_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts(string? search, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Products
                .Include(p => p.Loai)
                .Select(p => new
                {
                    p.ProductID,
                    p.Name,
                    p.ChiTiet,
                    p.Gia,
                    p.HinhAnh,
                    LoaiID = p.Loai.LoaiID,
                    TenLoai = p.Loai.TenLoai
                });

            // Lọc theo tên sản phẩm
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            // Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Gia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Gia <= maxPrice.Value);
            }

            var products = await query.ToListAsync();
            return Ok(products);
        }



        // Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.Include(p => p.Loai).FirstOrDefaultAsync(p => p.ProductID == id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }
            return product;
        }
        //
    

        [HttpGet("loai/{id}")]
        public async Task<ActionResult<Product>> GetProductAndLoai(int id)
        {
            var product = await _context.Products.Include(p => p.Loai).Where(p => p.LoaiID == id).ToListAsync();
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }
            return Ok(product);
        }

        // Tạo sản phẩm mới
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            var category = _context.Loais.Find(product.LoaiID);
            if (category == null)
            {
                return BadRequest("Invalid CategoryId.");
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // Xóa sản phẩm
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.ProductID == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
    }
}
