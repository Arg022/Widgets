using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class VideogameService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public VideogameService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Videogame> AddAsync(
        string title,
        string platform,
        string? developer   = null,
        string? publisher   = null,
        int?    releaseYear = null,
        string? coverPath   = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = new Videogame
        {
            Id          = Guid.NewGuid(),
            Title       = title,
            Platform    = platform,
            Developer   = developer,
            Publisher   = publisher,
            ReleaseYear = releaseYear,
            CoverPath   = coverPath,
            AddedAt     = DateTime.UtcNow
        };

        db.Videogames.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<Videogame?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Videogames
            .Include(v => v.Genres)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<Videogame>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Videogames
            .Include(v => v.Genres)
            .OrderBy(v => v.Title)
            .ToListAsync();
    }

    public async Task UpdateAsync(Videogame item)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Videogames.Update(item);
        await db.SaveChangesAsync();
    }
}