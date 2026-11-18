
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Provincias.Models;

public class HabitanteController : Controller
{
    private readonly ProvinciasDbCntext _context;

    public HabitanteController(ProvinciasDbCntext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var habitantes = await _context.Habitantes
            .Include(h => h.Canton)
            .ThenInclude(c => c.Provincia)
            .ToListAsync();
        return View(habitantes);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var habitante = await _context.Habitantes
            .Include(h => h.Canton)
            .ThenInclude(c => c.Provincia)
            .FirstOrDefaultAsync(h => h.Id == id);
        if (habitante == null)
        {
            return NotFound();
        }

        return View(habitante);
    }

    public async Task<IActionResult> Create()
    {
        await LoadRelatedDataAsync();
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetCantones(int provinciaId)
    {
        var cantones = await _context.Cantones
            .Where(c => c.ProvinciaId == provinciaId)
            .OrderBy(c => c.Nombre)
            .Select(c => new { c.Id, c.Nombre })
            .ToListAsync();
        return Json(cantones);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Nombre,Apellido,Fecha_Nacimiento,CorreoElectronico,CantonId")] Habitante habitante,
        int? provinciaId)
    {
        await ValidateCantonAsync(habitante.CantonId, provinciaId);
        if (ModelState.IsValid)
        {
            _context.Add(habitante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await LoadRelatedDataAsync(provinciaId, habitante.CantonId);
        return View(habitante);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var habitante = await _context.Habitantes
            .Include(h => h.Canton)
            .FirstOrDefaultAsync(h => h.Id == id);
        if (habitante == null)
        {
            return NotFound();
        }

        await LoadRelatedDataAsync(habitante.Canton.ProvinciaId, habitante.CantonId);
        return View(habitante);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Nombre,Apellido,Fecha_Nacimiento,CorreoElectronico,CantonId")] Habitante habitante,
        int? provinciaId)
    {
        if (id != habitante.Id)
        {
            return NotFound();
        }

        await ValidateCantonAsync(habitante.CantonId, provinciaId);
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(habitante);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitanteExists(habitante.Id))
                {
                    return NotFound();
                }

                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        await LoadRelatedDataAsync(provinciaId, habitante.CantonId);
        return View(habitante);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var habitante = await _context.Habitantes
            .Include(h => h.Canton)
            .ThenInclude(c => c.Provincia)
            .FirstOrDefaultAsync(h => h.Id == id);
        if (habitante == null)
        {
            return NotFound();
        }

        return View(habitante);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var habitante = await _context.Habitantes.FindAsync(id);
        if (habitante != null)
        {
            _context.Habitantes.Remove(habitante);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task LoadRelatedDataAsync(int? provinciaId = null, int? cantonId = null)
    {
        var provincias = await _context.Provincias
            .OrderBy(p => p.Nombre)
            .ToListAsync();
        var cantones = provinciaId.HasValue
            ? await _context.Cantones
                .Where(c => c.ProvinciaId == provinciaId.Value)
                .OrderBy(c => c.Nombre)
                .ToListAsync()
            : new List<Canton>();

        ViewBag.Provincias = new SelectList(provincias, "Id", "Nombre", provinciaId);
        ViewBag.Cantones = new SelectList(cantones, "Id", "Nombre", cantonId);
    }

    private async Task ValidateCantonAsync(int cantonId, int? provinciaId)
    {
        if (!provinciaId.HasValue || !await _context.Cantones.AnyAsync(
                c => c.Id == cantonId && c.ProvinciaId == provinciaId.Value))
        {
            ModelState.AddModelError(nameof(Habitante.CantonId),
                "Seleccione un cantón perteneciente a la provincia indicada.");
        }
    }

    private bool HabitanteExists(int id)
    {
        return _context.Habitantes.Any(e => e.Id == id);
    }
}
