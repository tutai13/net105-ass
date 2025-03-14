using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ass_api.Data;
using ass_api.Models;

namespace ass_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả loại sản phẩm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loai>>> GetLoais()
        {
            return await _context.Loais.ToListAsync();
        }

        // Lấy loại sản phẩm theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Loai>> GetLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);
            if (loai == null)
            {
                return NotFound(new { message = "Không tìm thấy loại sản phẩm." });
            }
            return loai;
        }

        // Tạo loại sản phẩm mới
        [HttpPost]
        public async Task<ActionResult<Loai>> CreateLoai(Loai loai)
        {
            _context.Loais.Add(loai);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoai), new { id = loai.LoaiID }, loai);
        }

        // Xóa loại sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoai(int id)
        {
            var loai = await _context.Loais.FindAsync(id);
            if (loai == null)
            {
                return NotFound(new { message = "Không tìm thấy loại sản phẩm." });
            }

            _context.Loais.Remove(loai);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Loai loai)
        {
            if (id != loai.LoaiID)
            {
                return BadRequest();
            }

            _context.Entry(loai).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Loais.Any(p => p.LoaiID == id))
                {
                    return NotFound();
                }
                throw;
            }
            return NoContent();
        }
    }
}
