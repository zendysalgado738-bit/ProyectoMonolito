using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public AutoresController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var autores = await _db.Autores.ToListAsync();
        return Ok(autores);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Autor autor)
    {
        // Lógica de negocio mezclada directamente en el controlador (a propósito)
        if (string.IsNullOrWhiteSpace(autor.Nombre))
            return BadRequest("El nombre del autor es obligatorio.");

        _db.Autores.Add(autor);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = autor.Id }, autor);
    }
}