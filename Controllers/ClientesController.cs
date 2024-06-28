using System;
using System.Collections.Generic;
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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clientes.Include(c => c.Equipa);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes
                .Include(c => c.Equipa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {         // procurar os dados das Equipas
                  // para os apresentar na 'dropdown' da interface
                  // em SQL: SELECT * FROM Cursos ORDER BY Nome
                  // em LINQ: _context.Equipas.OrderBy(c=>c.Nome)
            ViewData["EquipaFK"] = new SelectList(_context.Equipas, "Id", "Nome");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NumCliente,ValorCompra,DataCompra,EquipaFK,Id,Nome,DataNascimento,Telemovel,UserID")] Clientes cliente)
        {
            if (ModelState.IsValid)
            {

                // var. auxiliar
                bool haErros = false;

                if (cliente.EquipaFK == -1)
                {
                    // não escolhi equipa
                    ModelState.AddModelError("", "Escolha uma equipa, por favor.");
                    haErros = true;
                }


                if (ModelState.IsValid && !haErros)
                {

                    // transferir o valor de PropinasAux para Propinas
                    cliente.ValorCompra = Convert.ToDecimal(cliente.ValorCompraAux.Replace('.', ','));
                    cliente.ValorCompra = Convert.ToDecimal(cliente.ValorCompraAux.Replace('.', ','));

                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // se chego aqui é pq algo correu mal
                ViewData["EquipaFK"] = new SelectList(_context.Equipas.OrderBy(c => c.Nome), "Id", "Nome", cliente.EquipaFK);
            }
            return View(cliente);
        }

            // GET: Clientes/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes == null)
            {
                return NotFound();
            }
            ViewData["EquipaFK"] = new SelectList(_context.Equipas, "Id", "Nome", clientes.EquipaFK);
            return View(clientes);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumCliente,ValorCompra,DataCompra,EquipaFK,Id,Nome,DataNascimento,Telemovel,UserID")] Clientes clientes)
        {
            if (id != clientes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientesExists(clientes.Id))
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
            ViewData["EquipaFK"] = new SelectList(_context.Equipas, "Id", "Nome", clientes.EquipaFK);
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientes = await _context.Clientes
                .Include(c => c.Equipa)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clientes == null)
            {
                return NotFound();
            }

            return View(clientes);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientes = await _context.Clientes.FindAsync(id);
            if (clientes != null)
            {
                _context.Clientes.Remove(clientes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientesExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
