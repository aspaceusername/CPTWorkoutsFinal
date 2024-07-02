using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;

namespace CPTWorkouts.Controllers
{
    [Authorize(Roles = "Treinador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Compras
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Servico)
                .ToListAsync();
            return compras;
        }

        // GET: api/Compras/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compras>> GetCompras(int id)
        {
            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Servico)
                .FirstOrDefaultAsync(m => m.ClienteFK == id);

            if (compras == null)
            {
                return NotFound();
            }

            return compras;
        }

        // POST: api/Compras
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras(Compras compras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compras);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCompras), new { id = compras.ClienteFK }, compras);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Compras/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompras(int id, Compras compras)
        {
            if (id != compras.ClienteFK)
            {
                return BadRequest();
            }

            _context.Entry(compras).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComprasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Compras/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompras(int id)
        {
            var compras = await _context.Compras.FindAsync(id);
            if (compras == null)
            {
                return NotFound();
            }

            _context.Compras.Remove(compras);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComprasExists(int id)
        {
            return _context.Compras.Any(e => e.ClienteFK == id);
        }
    }
}
