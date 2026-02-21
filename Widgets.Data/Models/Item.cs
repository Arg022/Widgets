namespace Widgets.Data.Models;

public abstract class Item
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string? OriginalTitle { get; set; }
    public string? CoverPath { get; set; }
    public int? ReleaseYear { get; set; }
    public string Status { get; set; } = "Wishlist";
    public double? Rating { get; set; }
    public string? Notes { get; set; }
    public DateTime AddedAt { get; set; }

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}