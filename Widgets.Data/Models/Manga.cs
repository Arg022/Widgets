namespace Widgets.Data.Models;

public class Manga : Item
{
    public string Author { get; set; } = null!;
    public string? Artist { get; set; }

    /// <summary>Manga (JP) | Manhwa (KR) | Manhua (CN) | OEL</summary>
    public string Origin { get; set; } = "Manga";

    public int? TotalVolumes { get; set; }
    public int ReadVolumes { get; set; }

    public int? TotalChapters { get; set; }
    public int ReadChapters { get; set; }

    public bool IsCompleted { get; set; }
}