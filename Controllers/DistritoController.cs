using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Provincias.Models;

namespace Provincias.Controllers
{
    public class DistritoController : Controller
    {
        private readonly ProvinciasDbCntext _context;

        public DistritoController(ProvinciasDbCntext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var distritos = await _context.Distritos
                .Include(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .OrderBy(d => d.Nombre)
                .ToListAsync();
            return View(distritos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos
                .Include(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .Include(d => d.CentrosSalud)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCantonesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,CodigoPostal,AreaKm2,Poblacion,CantonId")] Distrito distrito)
        {
            ModelState.Remove(nameof(Distrito.Canton));
            if (ModelState.IsValid)
            {
                _context.Add(distrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadCantonesAsync(distrito.CantonId);
            return View(distrito);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos.FindAsync(id);
            if (distrito == null)
            {
                return NotFound();
            }

            await LoadCantonesAsync(distrito.CantonId);
            return View(distrito);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,CodigoPostal,AreaKm2,Poblacion,CantonId")] Distrito distrito)
        {
            if (id != distrito.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Distrito.Canton));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Distritos.AnyAsync(d => d.Id == distrito.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadCantonesAsync(distrito.CantonId);
            return View(distrito);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos
                .Include(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distrito = await _context.Distritos.FindAsync(id);
            if (distrito != null)
            {
                _context.Distritos.Remove(distrito);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCantonesAsync(int? cantonId = null)
        {
            var cantones = await _context.Cantones
                .Include(c => c.Provincia)
                .OrderBy(c => c.Provincia.Nombre)
                .ThenBy(c => c.Nombre)
                .Select(c => new
                {
                    c.Id,
                    NombreCompleto = c.Provincia.Nombre + " - " + c.Nombre
                })
                .ToListAsync();
            ViewBag.Cantones = new SelectList(cantones, "Id", "NombreCompleto", cantonId);
        }
    }
}
