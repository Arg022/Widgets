using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services.Services;

public class VideoWorkService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public VideoWorkService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<VideoWork> AddAsync(
        string title,
        bool    isSeries,
        string? director         = null,
        string? studio           = null,
        int?    releaseYear      = null,
        int?    durationMin      = null,
        int?    totalEpisodes    = null,
        string? streamingService = null,
        string? coverPath        = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = new VideoWork
        {
            Id               = Guid.NewGuid(),
            Title            = title,
            Director         = director,
            Studio           = studio,
            ReleaseYear      = releaseYear,
            DurationMin      = durationMin,
            TotalEpisodes    = isSeries ? totalEpisodes : null,
            StreamingService = streamingService,
            CoverPath        = coverPath,
            AddedAt          = DateTime.UtcNow
        };

        db.VideoWorks.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<VideoWork?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.VideoWorks
            .Include(v => v.Genres)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<List<VideoWork>> GetAllAsync(bool? seriesOnly = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var query = db.VideoWorks.Include(v => v.Genres).AsQueryable();

        if (seriesOnly is true)
            query = query.Where(v => v.TotalEpisodes != null);
        else if (seriesOnly is false)
            query = query.Where(v => v.TotalEpisodes == null);

        return await query.OrderBy(v => v.Title).ToListAsync();
    }

    public async Task UpdateAsync(VideoWork item)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.VideoWorks.Update(item);
        await db.SaveChangesAsync();
    }
}