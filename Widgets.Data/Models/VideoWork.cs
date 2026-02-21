namespace Widgets.Data.Models;

/// <summary>
/// Copre sia i film che le serie TV.
/// TotalEpisodes == null → film; TotalEpisodes > 0 → serie.
/// </summary>
public class VideoWork : Item
{
    public string? Director { get; set; }
    public string? Studio { get; set; }

    /// <summary>Durata in minuti (per i film) o durata media per episodio (per le serie).</summary>
    public int? DurationMin { get; set; }

    /// <summary>null = film, valore > 0 = serie TV.</summary>
    public int? TotalEpisodes { get; set; }
    public int WatchedEpisodes { get; set; }
    public string? StreamingService { get; set; }
}
