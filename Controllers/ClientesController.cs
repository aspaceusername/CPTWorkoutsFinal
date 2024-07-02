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
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> GetClientes()
        {
            var clientes = await _context.Clientes.Include(c => c.Equipa).ToListAsync();

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(clientes);
            }

            return View(clientes); // Assuming there's a View to display the list of clientes
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clientes>> GetClientes(int id)
        {
            var clientes = await _context.Clientes.Include(c => c.Equipa).FirstOrDefaultAsync(m => m.Id == id);

            if (clientes == null)
            {
                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return NotFound();
                }
                return NotFound(); // Assuming there's a View for displaying NotFound
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok(clientes);
            }

            return View(clientes); // Assuming there's a View to display the client details
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<Clientes>> PostClientes(Clientes cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();

                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return CreatedAtAction(nameof(GetClientes), new { id = cliente.Id }, cliente);
                }

                return RedirectToAction(nameof(GetClientes)); // Redirect to GET action to show updated list
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return BadRequest(ModelState);
            }

            return View(cliente); // Assuming there's a View to show the form again with validation errors
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClientes(int id, Clientes cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientesExists(id))
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
                return Ok(cliente);
            }

            return RedirectToAction(nameof(GetClientes)); // Redirect to GET action to show updated list
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientes(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok();
            }

            return RedirectToAction(nameof(GetClientes)); // Redirect to GET action to show updated list
        }

        private bool ClientesExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
