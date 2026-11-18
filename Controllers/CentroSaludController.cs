using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Provincias.Models;

namespace Provincias.Controllers
{
    public class CentroSaludController : Controller
    {
        private readonly ProvinciasDbCntext _context;

        public CentroSaludController(ProvinciasDbCntext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var centros = await _context.CentrosSalud
                .Include(c => c.Distrito)
                .ThenInclude(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
            return View(centros);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centro = await _context.CentrosSalud
                .Include(c => c.Distrito)
                .ThenInclude(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (centro == null)
            {
                return NotFound();
            }

            return View(centro);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDistritosAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Tipo,Telefono,Direccion,DistritoId")] CentroSalud centro)
        {
            ModelState.Remove(nameof(CentroSalud.Distrito));
            if (ModelState.IsValid)
            {
                _context.Add(centro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await LoadDistritosAsync(centro.DistritoId);
            return View(centro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centro = await _context.CentrosSalud.FindAsync(id);
            if (centro == null)
            {
                return NotFound();
            }

            await LoadDistritosAsync(centro.DistritoId);
            return View(centro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Tipo,Telefono,Direccion,DistritoId")] CentroSalud centro)
        {
            if (id != centro.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(CentroSalud.Distrito));
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.CentrosSalud.AnyAsync(c => c.Id == centro.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadDistritosAsync(centro.DistritoId);
            return View(centro);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centro = await _context.CentrosSalud
                .Include(c => c.Distrito)
                .ThenInclude(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (centro == null)
            {
                return NotFound();
            }

            return View(centro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centro = await _context.CentrosSalud.FindAsync(id);
            if (centro != null)
            {
                _context.CentrosSalud.Remove(centro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDistritosAsync(int? distritoId = null)
        {
            var distritos = await _context.Distritos
                .Include(d => d.Canton)
                .ThenInclude(c => c.Provincia)
                .OrderBy(d => d.Canton.Provincia.Nombre)
                .ThenBy(d => d.Canton.Nombre)
                .ThenBy(d => d.Nombre)
                .Select(d => new
                {
                    d.Id,
                    NombreCompleto = d.Canton.Provincia.Nombre + " - " + d.Canton.Nombre + " - " + d.Nombre
                })
                .ToListAsync();
            ViewBag.Distritos = new SelectList(distritos, "Id", "NombreCompleto", distritoId);
        }
    }
}
