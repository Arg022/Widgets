using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class GenreService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public GenreService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Tutti i generi. Se mediaType è specificato, restituisce i generi
    /// trasversali (MediaType == null) più quelli specifici per quel tipo.
    /// </summary>
    public async Task<List<Genre>> GetAllAsync(string? mediaType = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var query = db.Genres.AsQueryable();

        if (mediaType is not null)
            query = query.Where(g => g.MediaType == null || g.MediaType == mediaType);

        return await query.OrderBy(g => g.Name).ToListAsync();
    }

    /// <summary>Crea un nuovo genere se non esiste già (confronto case-insensitive).</summary>
    public async Task<Genre> CreateAsync(string name, string? mediaType = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var existing = await db.Genres
            .FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());

        if (existing is not null) return existing;

        var genre = new Genre
        {
            Id        = Guid.NewGuid(),
            Name      = name,
            MediaType = mediaType
        };

        db.Genres.Add(genre);
        await db.SaveChangesAsync();
        return genre;
    }

    /// <summary>Associa un genere a un item. Ignora se già associato.</summary>
    public async Task AddToItemAsync(Guid itemId, Guid genreId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = await db.Items
            .Include(i => i.Genres)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item is null) return;

        if (item.Genres.Any(g => g.Id == genreId)) return;

        var genre = await db.Genres.FindAsync(genreId);
        if (genre is null) return;

        item.Genres.Add(genre);
        await db.SaveChangesAsync();
    }

    /// <summary>Rimuove l'associazione tra un genere e un item.</summary>
    public async Task RemoveFromItemAsync(Guid itemId, Guid genreId)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = await db.Items
            .Include(i => i.Genres)
            .FirstOrDefaultAsync(i => i.Id == itemId);

        if (item is null) return;

        var genre = item.Genres.FirstOrDefault(g => g.Id == genreId);
        if (genre is null) return;

        item.Genres.Remove(genre);
        await db.SaveChangesAsync();
    }

    /// <summary>Elimina un genere e tutte le sue associazioni.</summary>
    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var genre = await db.Genres.FindAsync(id);
        if (genre is null) return;

        db.Genres.Remove(genre);
        await db.SaveChangesAsync();
    }
}