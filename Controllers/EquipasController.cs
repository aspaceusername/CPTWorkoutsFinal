using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Authorization;

namespace CPTWorkouts.Controllers
{
    [Authorize(Roles = "Treinador")]
    public class EquipasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EquipasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Equipas
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipas.ToListAsync());
        }

        // GET: Equipas/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipas
                .Include(e => e.ListaClientes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipa == null)
            {
                return NotFound();
            }

            return View(equipa);
        }

        // GET: Equipas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Equipas equipa, [FromForm] IFormFile ImagemLogo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagemLogo == null || ImagemLogo.Length <= 0)
                    {
                        ModelState.AddModelError("ImagemLogo", "Deve fornecer um logótipo");
                        return BadRequest(ModelState);
                    }

                    if (!(ImagemLogo.ContentType == "image/png" || ImagemLogo.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("ImagemLogo", "O logótipo deve ser uma imagem PNG ou JPEG.");
                        return BadRequest(ModelState);
                    }

                    // Generate a unique filename for the image
                    string nomeImagem = $"{Guid.NewGuid()}{Path.GetExtension(ImagemLogo.FileName)}";

                    // Save the image to wwwroot/Imagens folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", nomeImagem);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await ImagemLogo.CopyToAsync(stream);
                    }

                    // Resize the image if necessary (optional)
                    ResizeImage(imagePath);

                    // Save the Equipas object to database
                    equipa.Logotipo = nomeImagem;
                    _context.Add(equipa);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao salvar equipa: {ex.Message}");
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        // Optional: Resize image function
        private void ResizeImage(string imagePath)
        {
            using (var image = Image.Load(imagePath))
            {
                int novaLargura = 800;
                image.Mutate(x => x.Resize(novaLargura, 0));
                image.Save(imagePath);
            }
        }

        // GET: Equipas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipas.FindAsync(id);
            if (equipa == null)
            {
                return NotFound();
            }

            return View(equipa);
        }

        // POST: Equipas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Logotipo")] Equipas equipa)
        {
            if (id != equipa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipasExists(equipa.Id))
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
            return View(equipa);
        }

        // GET: Equipas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipas
                .FirstOrDefaultAsync(m => m.Id == id);

            if (equipa == null)
            {
                return NotFound();
            }

            return View(equipa);
        }

        // POST: Equipas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipa = await _context.Equipas.FindAsync(id);
            if (equipa != null)
            {
                _context.Equipas.Remove(equipa);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EquipasExists(int id)
        {
            return _context.Equipas.Any(e => e.Id == id);
        }
    }
}
