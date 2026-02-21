namespace Widgets.Data.Models;

public class Book : Item
{
    public string Author { get; set; } = null!;
    public string? Publisher { get; set; }
    public string? Isbn { get; set; }
    public int? Pages { get; set; }
    public int CurrentPage { get; set; }
    public string? Language { get; set; }
}
