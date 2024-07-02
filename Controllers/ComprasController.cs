using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(compras);
            }

            return View(compras); // Assuming there's a View to display the list of compras
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
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return NotFound();
                }
                return NotFound(); // Assuming there's a View for displaying NotFound
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(compras);
            }

            return View(compras); // Assuming there's a View to display the compra details
        }

        // POST: api/Compras
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras(Compras compras)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compras);
                await _context.SaveChangesAsync();

                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return CreatedAtAction(nameof(GetCompras), new { id = compras.ClienteFK }, compras);
                }

                return RedirectToAction(nameof(GetCompras)); // Redirect to GET action to show updated list
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return BadRequest(ModelState);
            }

            return View(compras); // Assuming there's a View to show the form again with validation errors
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

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(compras);
            }

            return RedirectToAction(nameof(GetCompras)); // Redirect to GET action to show updated list
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

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok();
            }

            return RedirectToAction(nameof(GetCompras)); // Redirect to GET action to show updated list
        }

        private bool ComprasExists(int id)
        {
            return _context.Compras.Any(e => e.ClienteFK == id);
        }
    }
}
