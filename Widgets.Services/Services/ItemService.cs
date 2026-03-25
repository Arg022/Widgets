using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services.Services;

public class ItemService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ItemService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    /// <summary>Tutti gli item.</summary>
    public async Task<List<Item>> GetAllAsync(string? mediaType = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var query = db.Items
            .Include(i => i.Genres)
            .AsQueryable();
        
        return await query.OrderBy(i => i.Title).ToListAsync();
    }

    /// <summary>Singolo item per Id, null se non trovato.</summary>
    public async Task<Item?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Items
            .Include(i => i.Genres)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>Ricerca per titolo (case-insensitive, parziale).</summary>
    public async Task<List<Item>> SearchAsync(string query)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Items
            .Include(i => i.Genres)
            .Where(i => EF.Functions.Like(i.Title, $"%{query}%")
                     || EF.Functions.Like(i.OriginalTitle ?? "", $"%{query}%"))
            .OrderBy(i => i.Title)
            .ToListAsync();
    }

    /// <summary>Aggiorna i campi comuni: status, rating, note, copertina.</summary>
    public async Task UpdateAsync(Item item)
    {
        await using var db = await _factory.CreateDbContextAsync();

        db.Items.Update(item);
        await db.SaveChangesAsync();
    }

    /// <summary>Elimina un item e tutte le sue righe figlie (cascade).</summary>
    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = await db.Items.FindAsync(id);
        if (item is null) return;

        db.Items.Remove(item);
        await db.SaveChangesAsync();
    }
}