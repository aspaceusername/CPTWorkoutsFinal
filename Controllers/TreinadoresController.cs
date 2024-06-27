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
    public class TreinadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreinadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treinadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Treinadores.ToListAsync());
        }

        // GET: Treinadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treinadores = await _context.Treinadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treinadores == null)
            {
                return NotFound();
            }

            return View(treinadores);
        }
        // GET: Treinadores/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Treinadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,DataNascimento,Telemovel,UserID")] Treinadores treinadores)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treinadores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treinadores);
        }
        // GET: Treinadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treinadores = await _context.Treinadores.FindAsync(id);
            if (treinadores == null)
            {
                return NotFound();
            }
            return View(treinadores);
        }
        // POST: Treinadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,DataNascimento,Telemovel,UserID")] Treinadores treinadores)
        {
            if (id != treinadores.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treinadores);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreinadoresExists(treinadores.Id))
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
            return View(treinadores);
        }
        // GET: Treinadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treinadores = await _context.Treinadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treinadores == null)
            {
                return NotFound();
            }

            return View(treinadores);
        }
        // POST: Treinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treinadores = await _context.Treinadores.FindAsync(id);
            if (treinadores != null)
            {
                _context.Treinadores.Remove(treinadores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinadoresExists(int id)
        {
            return _context.Treinadores.Any(e => e.Id == id);
        }
    }
}
