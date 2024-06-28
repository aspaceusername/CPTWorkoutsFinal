using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Referência à BD do projecto
        /// </summary>
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public EquipasController(
           ApplicationDbContext context,
           IWebHostEnvironment webHostEnvironment)
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

            var equipas = await _context.Equipas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipas == null)
            {
                return NotFound();
            }

            return View(equipas);
        }

        // GET: Equipas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Equipas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Logotipo")] Equipas equipa, IFormFile ImagemLogo)
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

            // avalia se os dados recebido do browser estão
            // de acordo com o Model
            if (ModelState.IsValid)
            {
                // vars auxiliares
                string nomeImagem = "";
                bool haImagem = false;

                // há ficheiro?
                if (ImagemLogo == null)
                {
                    // não há
                    // crio msg de erro
                    ModelState.AddModelError("",
                       "Deve fornecer um logótipo");
                    // devolver controlo à View
                    return View(equipa);
                }
                else
                {
                    // há ficheiro, mas é uma imagem?
                    if (!(ImagemLogo.ContentType == "image/png" ||
                         ImagemLogo.ContentType == "image/jpeg"
                       ))
                    {
                        // não
                        // vamos usar uma imagem pre-definida
                        equipa.Logotipo = "logoEquipa.png";
                    }
                    else
                    {
                        // há imagem
                        haImagem = true;
                        // gerar nome imagem
                        Guid g = Guid.NewGuid();
                        nomeImagem = g.ToString();
                        string extensaoImagem = Path.GetExtension(ImagemLogo.FileName).ToLowerInvariant();
                        nomeImagem += extensaoImagem;
                        // guardar o nome do ficheiro na BD
                        equipa.Logotipo = nomeImagem;
                    }
                }


                // adiciona à BD os dados vindos da View
                _context.Add(equipa);
                // Commit
                await _context.SaveChangesAsync();

                // guardar a imagem do logótipo
                if (haImagem)
                {
                    // determinar o local de armazenamento da imagem
                    string localizacaoImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens");

                    // verifica se o diretório existe; se não existir, cria
                    if (!Directory.Exists(localizacaoImagem))
                    {
                        Directory.CreateDirectory(localizacaoImagem);
                    }

                    // caminho completo onde a imagem será armazenada
                    localizacaoImagem = Path.Combine(localizacaoImagem, nomeImagem);

                    // guardar a imagem no Disco Rígido
                    using var stream = new FileStream(localizacaoImagem, FileMode.Create);

                    // carregar a imagem do stream
                    using (var image = Image.Load(ImagemLogo.OpenReadStream()))
                    {
                        // redimensionar a imagem para o tamanho desejado (exemplo: largura de 800 pixels)
                        int novaLargura = 800;
                        image.Mutate(x => x.Resize(novaLargura, 0)); // 0 mantém a proporção original

                        // salvar a imagem redimensionada no stream
                        image.Save(stream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    }
                }




                // redireciona o utilizador para a página de 'início'
                // dos Cursos
                return RedirectToAction(nameof(Index));
            }
            // se cheguei aqui é pq alguma coisa correu mal
            // devolve controlo à View, apresentando os dados recebidos
            return View(equipa);
        }

        // GET: Equipas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipas = await _context.Equipas.FindAsync(id);
            if (equipas == null)
            {
                return NotFound();
            }
            return View(equipas);
        }

        // POST: Equipas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Logotipo")] Equipas equipas)
        {
            if (id != equipas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipasExists(equipas.Id))
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
            return View(equipas);
        }

        // GET: Equipas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipas = await _context.Equipas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipas == null)
            {
                return NotFound();
            }

            return View(equipas);
        }

        // POST: Equipas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipas = await _context.Equipas.FindAsync(id);
            if (equipas != null)
            {
                _context.Equipas.Remove(equipas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipasExists(int id)
        {
            return _context.Equipas.Any(e => e.Id == id);
        }
    }
}
