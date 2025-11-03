using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data;

public class MyMoviesDbContext : DbContext
{
    public MyMoviesDbContext(DbContextOptions<MyMoviesDbContext> options) : base(options) { }
    public DbSet<Movie> Movies { get; set; }
}