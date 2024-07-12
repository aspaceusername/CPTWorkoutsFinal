using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Authorization;

namespace CPTWorkouts.Controllers
{
    [Authorize(Roles = "Treinador")]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Servico);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? clienteFK, int? servicoFK)
        {
            if (clienteFK == null || servicoFK == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Servico)
                .FirstOrDefaultAsync(m => m.ClienteFK == clienteFK && m.ServicoFK == servicoFK);
            if (compras == null)
            {
                return NotFound();
            }

            return View(compras);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["ClienteFK"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "Id", "Nome");
            return View();
        }

        // POST: Compras/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DataCompra,ValorCompraAux,ServicoFK,ClienteFK")] Compras compras)
        {
            if (ModelState.IsValid)
            {
                compras.ValorCompra = Convert.ToDecimal(compras.ValorCompraAux.Replace('.', ',')); // Convert ValorCompraAux to decimal
                _context.Add(compras);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteFK"] = new SelectList(_context.Clientes, "Id", "Nome", compras.ClienteFK);
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "Id", "Nome", compras.ServicoFK);
            return View(compras);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? clienteFK, int? servicoFK)
        {
            if (clienteFK == null || servicoFK == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras
                .FirstOrDefaultAsync(c => c.ClienteFK == clienteFK && c.ServicoFK == servicoFK);

            if (compras == null)
            {
                return NotFound();
            }

            ViewData["ClienteFK"] = new SelectList(_context.Clientes, "Id", "Nome", compras.ClienteFK);
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "Id", "Nome", compras.ServicoFK);

            return View(compras);
        }

        // POST: Compras/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int clienteFK, int servicoFK, [Bind("DataCompra,ValorCompraAux,ServicoFK,ClienteFK")] Compras compras)
        {
            if (clienteFK != compras.ClienteFK || servicoFK != compras.ServicoFK)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    compras.ValorCompra = Convert.ToDecimal(compras.ValorCompraAux.Replace('.', ',')); // Convert ValorCompraAux to decimal
                    _context.Update(compras);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprasExists(clienteFK, servicoFK))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteFK"] = new SelectList(_context.Clientes, "Id", "Nome", compras.ClienteFK);
            ViewData["ServicoFK"] = new SelectList(_context.Servicos, "Id", "Nome", compras.ServicoFK);

            return View(compras);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? clienteFK, int? servicoFK)
        {
            if (clienteFK == null || servicoFK == null)
            {
                return NotFound();
            }

            var compras = await _context.Compras
                .Include(c => c.Cliente)
                .Include(c => c.Servico)
                .FirstOrDefaultAsync(m => m.ClienteFK == clienteFK && m.ServicoFK == servicoFK);
            if (compras == null)
            {
                return NotFound();
            }

            return View(compras);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int clienteFK, int servicoFK)
        {
            try
            {
                var compras = await _context.Compras
                    .FirstOrDefaultAsync(m => m.ClienteFK == clienteFK && m.ServicoFK == servicoFK);

                if (compras == null)
                {
                    return NotFound();
                }

                _context.Compras.Remove(compras);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return RedirectToAction(nameof(Index)); // Redirect to index on error
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ComprasExists(int clienteFK, int servicoFK)
        {
            return _context.Compras.Any(e => e.ClienteFK == clienteFK && e.ServicoFK == servicoFK);
        }
    }
}
