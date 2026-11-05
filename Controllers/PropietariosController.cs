using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using veterinaria_api.Models;

namespace veterinaria_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropietariosController : ControllerBase
{
    private readonly VeterinariaContext _context;

    public PropietariosController(VeterinariaContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Propietario>>> Get()
    {
        var propietarios = await _context.Propietarios
            .AsNoTracking()
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();

        return Ok(propietarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Propietario>> Get(int id)
    {
        var propietario = await _context.Propietarios
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdPropietario == id);

        return propietario is null ? NotFound() : Ok(propietario);
    }

    [HttpGet("{id:int}/mascotas")]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas(int id)
    {
        if (!await _context.Propietarios.AnyAsync(p => p.IdPropietario == id))
            return NotFound();

        var mascotas = await _context.Mascotas
            .AsNoTracking()
            .Where(m => m.IdPropietario == id)
            .OrderBy(m => m.Nombre)
            .ToListAsync();

        return Ok(mascotas);
    }

    [HttpPost]
    public async Task<ActionResult<Propietario>> Post(Propietario propietario)
    {
        propietario.Dni = propietario.Dni.Trim();

        if (await _context.Propietarios.AnyAsync(p => p.Dni == propietario.Dni))
            return Conflict(new ProblemDetails
            {
                Title = "DNI duplicado",
                Detail = $"Ya existe un propietario registrado con el DNI {propietario.Dni}.",
                Status = StatusCodes.Status409Conflict
            });

        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = propietario.IdPropietario }, propietario);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, Propietario propietario)
    {
        if (id != propietario.IdPropietario)
            return BadRequest("El identificador de la ruta no coincide con el cuerpo de la solicitud.");

        var actual = await _context.Propietarios.FindAsync(id);
        if (actual is null)
            return NotFound();

        propietario.Dni = propietario.Dni.Trim();
        if (await _context.Propietarios.AnyAsync(p => p.Dni == propietario.Dni && p.IdPropietario != id))
            return Conflict(new ProblemDetails
            {
                Title = "DNI duplicado",
                Detail = $"Ya existe otro propietario registrado con el DNI {propietario.Dni}.",
                Status = StatusCodes.Status409Conflict
            });

        actual.Nombres = propietario.Nombres;
        actual.Apellidos = propietario.Apellidos;
        actual.Dni = propietario.Dni;
        actual.Telefono = propietario.Telefono;
        actual.Correo = propietario.Correo;
        actual.Direccion = propietario.Direccion;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario is null)
            return NotFound();

        if (await _context.Mascotas.AnyAsync(m => m.IdPropietario == id))
            return Conflict(new ProblemDetails
            {
                Title = "Propietario con mascotas",
                Detail = "No se puede eliminar un propietario que todavía tiene mascotas registradas.",
                Status = StatusCodes.Status409Conflict
            });

        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
