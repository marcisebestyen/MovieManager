using System.Text.Json;
using BlazorApp1.Data;

namespace BlazorApp1.Services;

public class MovieService
{
    private readonly string _jsonFilePath;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public MovieService(IWebHostEnvironment env)
    {
        _jsonFilePath = Path.Combine(env.ContentRootPath, "movies.json");
    }

    private async Task<List<Movie>> ReadMoviesFromFileAsync()
    {
        if (!File.Exists(_jsonFilePath))
        {
            return new List<Movie>();
        }

        var json = await File.ReadAllTextAsync(_jsonFilePath);
        
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<Movie>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<Movie>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Movie>();
        }
        catch (JsonException)
        {
            var emptyList = new List<Movie>();
            await WriteMoviesToFileAsync(emptyList);
            return emptyList;
        }
    }

    private async Task WriteMoviesToFileAsync(List<Movie> movies)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var json = JsonSerializer.Serialize(movies, options);
        
        await _semaphore.WaitAsync(); 
        try
        {
            await File.WriteAllTextAsync(_jsonFilePath, json);
        }
        finally
        {
            _semaphore.Release(); 
        }
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        return await ReadMoviesFromFileAsync();
    }

    public async Task<Movie?> GetMovieByIdAsync(int id)
    {
        var movies = await ReadMoviesFromFileAsync();
        return movies.FirstOrDefault(m => m.Id == id);
    }

    public async Task AddMovieAsync(Movie movie)
    {
        var movies = await ReadMoviesFromFileAsync();
        movie.Id = movies.Any() ? movies.Max(m => m.Id) + 1 : 1;
        movies.Add(movie);
        await WriteMoviesToFileAsync(movies);
    }

    public async Task UpdateMovieAsync(Movie movieToUpdate)
    {
        var movies = await ReadMoviesFromFileAsync();
        var movie = movies.FirstOrDefault(m => m.Id == movieToUpdate.Id);
        if (movie != null)
        {
            movie.Title = movieToUpdate.Title;
            movie.Director = movieToUpdate.Director;
            movie.Year = movieToUpdate.Year;
            movie.Rating = movieToUpdate.Rating;
            movie.Genre = movieToUpdate.Genre;
            await WriteMoviesToFileAsync(movies);
        }
    }

    public async Task DeleteMovieAsync(int id)
    {
        var movies = await ReadMoviesFromFileAsync();
        var movie = movies.FirstOrDefault(m => m.Id == id);
        if (movie != null)
        {
            movies.Remove(movie);
            await WriteMoviesToFileAsync(movies);
        }
    }
}