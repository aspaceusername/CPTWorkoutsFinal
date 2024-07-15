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
            // a anotação [Bind] informa o servidor de quais os atributos
            // que devem ser lidos do objeto que vem do browser

            /* Guardar a imagem no disco rígido do Servidor
             * Algoritmo
             * 1- há ficheiro?
             *    1.1 - não
             *          devolvo controlo ao browser
             *          com mensagem de erro
             *    1.2 - sim
             *          Será imagem (JPG,JPEG,PNG)?
             *          1.2.1 - não
             *                  uso logótipo pre-definido
             *          1.2.2 - sim
             *                  - determinar o nome da imagem
             *                  - guardar esse nome na BD
             *                  - guardar o ficheir no disco rígido
             */

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagemLogo == null || ImagemLogo.Length <= 0)
                    {
                        ModelState.AddModelError("ImagemLogo", "Deve fornecer um logotipo");
                        return BadRequest(ModelState);
                    }

                    if (!(ImagemLogo.ContentType == "image/png" || ImagemLogo.ContentType == "image/jpeg"))
                    {
                        ModelState.AddModelError("ImagemLogo", "O logotipo deve ser uma imagem PNG ou JPEG.");
                        return BadRequest(ModelState);
                    }

                    //gerar um nome de ficheiro
                    string nomeImagem = $"{Guid.NewGuid()}{Path.GetExtension(ImagemLogo.FileName)}";

                    // guardar a imagem no wwwroot/imagens
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", nomeImagem);

                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await ImagemLogo.CopyToAsync(stream);
                    }

                    // Redimensionar a imagem
                    ResizeImage(imagePath);

                    // gaurdar o objecto equipas na base de dados
                    equipa.Logotipo = nomeImagem;
                    _context.Add(equipa);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro ao guardar equipa: {ex.Message}");
                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        // redimensionar imagem
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
