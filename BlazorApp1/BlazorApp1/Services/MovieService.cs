using BlazorApp1.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class MovieService
    {
        private readonly MyMoviesDbContext _context;

        public MovieService(MyMoviesDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            return await _context.Movies.AsNoTracking().ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movieToUpdate)
        {
            var existing = await _context.Movies.FindAsync(movieToUpdate.Id);
            if (existing is not null)
            {
                existing.Title = movieToUpdate.Title;
                existing.Director = movieToUpdate.Director;
                existing.Year = movieToUpdate.Year;
                existing.Rating = movieToUpdate.Rating;
                existing.Genre = movieToUpdate.Genre;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie is not null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
}