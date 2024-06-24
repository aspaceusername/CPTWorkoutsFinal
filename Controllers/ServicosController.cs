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
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Servicos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Servicos.ToListAsync());
        }

        // GET: Servicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .Include(s => s.ListaTreinadores)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servicos == null)
            {
                return NotFound();
            }

            return View(servicos);
        }


        // GET: Servicos/Create
        public IActionResult Create()
        {
            // obter a lista de Treinadores existentes na BD
            ViewData["ListaTreinadores"] = _context.Treinadores.OrderBy(p => p.Nome).ToList();
            return View();
        }

        // POST: Servicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Preco")] Servicos servico, int[] listaIdsTreinadores)
        {
            // VALIDAR SE FOI ESCOLHIDO PELO MENOS UM Treinador

            // PQ HÁ Treinador(ES)
            var listaTreinadores = new List<Treinadores>();
            foreach (var treinadorId in listaIdsTreinadores)
            {
                //var treinador = _context.Treinadores.FirstOrDefault(t => t.Id == treinadorId);
                var treinador = await _context.Treinadores.FindAsync(treinadorId);
                if (treinador != null)
                {
                    listaTreinadores.Add(treinador);
                }
            }


            if (listaTreinadores != null)
            {
                servico.ListaTreinadores = listaTreinadores;
            }
            else
            {
                // se chego aqui, houve tratalhada feita no browser
                // gerar mensagem de erro
                // notificar utilizador
                // enviar controlo à VIEW
            }

            if (ModelState.IsValid)
            {

                _context.Add(servico);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ListaTreinadores"] = _context.Treinadores.ToList();
            return View(servico);
        }

        // GET: Servicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicos = await _context.Servicos.FindAsync(id);
            if (servicos == null)
            {
                return NotFound();
            }
            return View(servicos);
        }

        // POST: Servicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Preco")] Servicos servicos)
        {
            if (id != servicos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicosExists(servicos.Id))
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
            return View(servicos);
        }

        // GET: Servicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servicos = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servicos == null)
            {
                return NotFound();
            }

            return View(servicos);
        }

        // POST: Servicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servicos = await _context.Servicos.FindAsync(id);
            if (servicos != null)
            {
                _context.Servicos.Remove(servicos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicosExists(int id)
        {
            return _context.Servicos.Any(e => e.Id == id);
        }
    }
}
