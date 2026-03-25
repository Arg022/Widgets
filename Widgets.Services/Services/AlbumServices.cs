using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services.Services;

public class AlbumService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public AlbumService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Album> AddAsync(
        string title,
        string artist,
        string? label       = null,
        int?    releaseYear = null,
        int?    trackCount  = null,
        int?    durationMin = null,
        string  format      = "Digital",
        string? coverPath   = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = new Album
        {
            Id          = Guid.NewGuid(),
            Title       = title,
            Artist      = artist,
            Label       = label,
            ReleaseYear = releaseYear,
            TrackCount  = trackCount,
            DurationMin = durationMin,
            Format      = format,
            CoverPath   = coverPath,
            AddedAt     = DateTime.UtcNow
        };

        db.Albums.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<Album?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Albums
            .Include(a => a.Genres)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Album>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Albums
            .Include(a => a.Genres)
            .OrderBy(a => a.Artist)
            .ThenBy(a => a.Title)
            .ToListAsync();
    }

    public async Task UpdateAsync(Album item)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Albums.Update(item);
        await db.SaveChangesAsync();
    }
}