namespace Widgets.Data.Models;

public class Videogame : Item
{
    public string Platform { get; set; } = null!;
    public string? Developer { get; set; }
    public string? Publisher { get; set; }
    public double HoursPlayed { get; set; }

    /// <summary>Percentuale di completamento 0–100</summary>
    public int Completion { get; set; }
}
