namespace Widgets.Data.Models;

public class Album : Item
{
    public string Artist { get; set; } = null!;
    public string? Label { get; set; }
    public int? TrackCount { get; set; }
    public int? DurationMin { get; set; }

    /// <summary>Digital | Vinyl | CD | Cassette</summary>
    public string Format { get; set; } = "Digital";
}