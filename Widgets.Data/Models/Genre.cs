namespace Widgets.Data.Models;

public class Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;

    /// <summary>
    /// Se null il genere è trasversale a tutti i media (es. Fantasy, Thriller).
    /// Altrimenti limita il genere a un tipo specifico (es. "Game", "Manga").
    /// </summary>
    public string? MediaType { get; set; }

    public ICollection<Item> Items { get; set; } = new List<Item>();
}