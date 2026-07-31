using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options) { }

        public DbSet<Autor> Autores => Set<Autor>();

        public DbSet<Libro> Libros => Set<Libro>();
    }
}
