using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class MangaService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public MangaService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Manga> AddAsync(
        string  title,
        string  author,
        string  origin        = "Manga",
        string? artist        = null,
        int?    totalVolumes  = null,
        int?    totalChapters = null,
        int?    releaseYear   = null,
        string? coverPath     = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = new Manga
        {
            Id            = Guid.NewGuid(),
            Title         = title,
            Author        = author,
            Artist        = artist,
            Origin        = origin,
            TotalVolumes  = totalVolumes,
            TotalChapters = totalChapters,
            ReleaseYear   = releaseYear,
            CoverPath     = coverPath,
            AddedAt       = DateTime.UtcNow
        };

        db.Mangas.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<Manga?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Mangas
            .Include(m => m.Genres)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Manga>> GetAllAsync(string? origin = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var query = db.Mangas.Include(m => m.Genres).AsQueryable();

        if (origin is not null)
            query = query.Where(m => m.Origin == origin);

        return await query.OrderBy(m => m.Title).ToListAsync();
    }

    public async Task UpdateAsync(Manga item)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Mangas.Update(item);
        await db.SaveChangesAsync();
    }

    /// <summary>Aggiorna il progresso di lettura.</summary>
    public async Task UpdateProgressAsync(Guid id, int readVolumes, int readChapters)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = await db.Mangas.FindAsync(id);
        if (item is null) return;

        item.ReadVolumes  = readVolumes;
        item.ReadChapters = readChapters;

        await db.SaveChangesAsync();
    }
}