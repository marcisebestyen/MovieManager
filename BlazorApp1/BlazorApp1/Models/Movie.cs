using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Data;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Director is required")]
    public string Director { get; set; } = string.Empty;

    [Required(ErrorMessage = "Year is required")]
    [Range(1886, 2030, ErrorMessage = "Year must be between 1886 and 2030")]
    public int? Year { get; set; } 

    [Required(ErrorMessage = "Rating is required")]
    [Range(0.1, 10.0, ErrorMessage = "Rating must be between 0.1 and 10.0")]
    public decimal? Rating { get; set; } 

    [Required(ErrorMessage = "Genre is required")]
    public string Genre { get; set; } = string.Empty;
}