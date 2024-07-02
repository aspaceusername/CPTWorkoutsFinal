using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using CPTWorkouts.Data;
using CPTWorkouts.Models;

namespace CPTWorkouts.Controllers
{
    [Authorize(Roles = "Treinador")]
    [Route("[controller]")]
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
        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var equipas = await _context.Equipas.ToListAsync();
            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Json(equipas);
            }
            return View(equipas);
        }

        // GET: Equipas/Details/5
        [HttpGet]
        [AllowAnonymous]
        [Route("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipas.FirstOrDefaultAsync(m => m.Id == id);
            if (equipa == null)
            {
                return NotFound();
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Json(equipa);
            }
            return View(equipa);
        }

        // GET: Equipas/Create
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipas/Create
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Logotipo")] Equipas equipa, IFormFile ImagemLogo)
        {
            if (ModelState.IsValid)
            {
                string nomeImagem = "";
                bool haImagem = false;

                if (ImagemLogo == null)
                {
                    ModelState.AddModelError("", "Deve fornecer um logótipo");
                    if (Request.Headers["Accept"].ToString().Contains("application/json"))
                    {
                        return BadRequest(ModelState);
                    }
                    return View(equipa);
                }
                else
                {
                    if (!(ImagemLogo.ContentType == "image/png" || ImagemLogo.ContentType == "image/jpeg"))
                    {
                        equipa.Logotipo = "logoEquipa.png";
                    }
                    else
                    {
                        haImagem = true;
                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(ImagemLogo.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        equipa.Logotipo = nomeImagem;
                    }
                }

                _context.Add(equipa);
                await _context.SaveChangesAsync();

                if (haImagem)
                {
                    string localizacaoImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens");
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }
                    localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);

                    using var stream = new FileStream(localizacaoImagem, FileMode.Create);
                    using (var image = Image.Load(ImagemLogo.OpenReadStream()))
                    {
                        int novaLargura = 800;
                        image.Mutate(x => x.Resize(novaLargura, 0));
                        image.Save(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    }
                }

                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return Ok(equipa);
                }

                return RedirectToAction(nameof(Index));
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return BadRequest(ModelState);
            }
            return View(equipa);
        }

        // GET: Equipas/Edit/5
        [HttpGet]
        [Route("Edit/{id}")]
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
        [Route("Edit/{id}")]
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

                if (Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return Ok(equipa);
                }
                return RedirectToAction(nameof(Index));
            }

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return BadRequest(ModelState);
            }
            return View(equipa);
        }

        // GET: Equipas/Delete/5
        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipa = await _context.Equipas.FirstOrDefaultAsync(m => m.Id == id);
            if (equipa == null)
            {
                return NotFound();
            }

            return View(equipa);
        }

        // POST: Equipas/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("DeleteConfirmed/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipa = await _context.Equipas.FindAsync(id);
            if (equipa != null)
            {
                _context.Equipas.Remove(equipa);
            }

            await _context.SaveChangesAsync();

            if (Request.Headers["Accept"].ToString().Contains("application/json"))
            {
                return Ok();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EquipasExists(int id)
        {
            return _context.Equipas.Any(e => e.Id == id);
        }
    }
}
