using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using veterinaria_api.Models;

namespace veterinaria_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MascotasController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public MascotasController(VeterinariaContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mascota>>> Get(string? especie, string? nombre)
    {
        IQueryable<Mascota> consulta = _context.Mascotas
            .Include(m => m.Propietario)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(especie))
            consulta = consulta.Where(m => m.Especie.Contains(especie));

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(m => m.Nombre.Contains(nombre));

        var mascotas = await consulta
            .OrderBy(m => m.Nombre)
            .ToListAsync();

        return Ok(mascotas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Mascota>> Get(int id)
    {
        var mascota = await _context.Mascotas
            .Include(m => m.Propietario)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.IdMascota == id);

        return mascota is null ? NotFound() : Ok(mascota);
    }

    [HttpPost]
    public async Task<ActionResult<Mascota>> Post(Mascota mascota)
    {
        if (!FechaNacimientoEsValida(mascota.FechaNacimiento))
            return BadRequest("La fecha de nacimiento no puede ser futura.");

        if (!await _context.Propietarios.AnyAsync(p => p.IdPropietario == mascota.IdPropietario))
        {
            ModelState.AddModelError(nameof(mascota.IdPropietario), "El propietario indicado no existe.");
            return ValidationProblem(ModelState);
        }

        mascota.Propietario = null;
        _context.Mascotas.Add(mascota);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = mascota.IdMascota }, mascota);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Mascota mascota)
    {
        if (id != mascota.IdMascota)
            return BadRequest("El identificador de la ruta no coincide con el cuerpo de la solicitud.");

        var actual = await _context.Mascotas.FindAsync(id);
        if (actual is null)
            return NotFound();

        if (!FechaNacimientoEsValida(mascota.FechaNacimiento))
            return BadRequest("La fecha de nacimiento no puede ser futura.");

        if (!await _context.Propietarios.AnyAsync(p => p.IdPropietario == mascota.IdPropietario))
        {
            ModelState.AddModelError(nameof(mascota.IdPropietario), "El propietario indicado no existe.");
            return ValidationProblem(ModelState);
        }

        actual.Nombre = mascota.Nombre;
        actual.Especie = mascota.Especie;
        actual.Raza = mascota.Raza;
        actual.Sexo = mascota.Sexo;
        actual.FechaNacimiento = mascota.FechaNacimiento;
        actual.Peso = mascota.Peso;
        actual.IdPropietario = mascota.IdPropietario;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mascota = await _context.Mascotas.FindAsync(id);
        if (mascota is null)
            return NotFound();

        _context.Mascotas.Remove(mascota);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static bool FechaNacimientoEsValida(DateOnly fechaNacimiento) =>
        fechaNacimiento <= DateOnly.FromDateTime(DateTime.Today);
}
