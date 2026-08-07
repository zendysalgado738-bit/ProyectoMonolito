using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public LibrosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libros = await _db.Libros.Include(l => l.Autor).ToListAsync();
        return Ok(libros);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _db.Libros.Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (libro is null) return NotFound();
        return Ok(libro);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Libro libro)
    {
        // Validación de negocio directamente aquí (monolítico a propósito)
        if (string.IsNullOrWhiteSpace(libro.Titulo))
            return BadRequest("El título es obligatorio.");
        if (libro.AnioPublicacion < 1440 || libro.AnioPublicacion > DateTime.Now.Year)
            return BadRequest("Año de publicación inválido.");

        var autorExiste = await _db.Autores.AnyAsync(a => a.Id == libro.AutorId);
        if (!autorExiste) return BadRequest("El autor especificado no existe.");

        _db.Libros.Add(libro);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = libro.Id }, libro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Libro libroActualizado)
    {
        var libro = await _db.Libros.FindAsync(id);
        if (libro is null) return NotFound();

        libro.Titulo = libroActualizado.Titulo;
        libro.AnioPublicacion = libroActualizado.AnioPublicacion;
        libro.AutorId = libroActualizado.AutorId;

        await _db.SaveChangesAsync();
        return Ok(libro);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await _db.Libros.FindAsync(id);
        if (libro is null) return NotFound();

        _db.Libros.Remove(libro);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
