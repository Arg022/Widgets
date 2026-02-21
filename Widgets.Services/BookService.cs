using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class BookService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public BookService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<Book> AddAsync(
        string  title,
        string  author,
        string? publisher   = null,
        string? isbn        = null,
        int?    pages       = null,
        string? language    = null,
        int?    releaseYear = null,
        string? coverPath   = null)
    {
        await using var db = await _factory.CreateDbContextAsync();

        var item = new Book
        {
            Id          = Guid.NewGuid(),
            Title       = title,
            Author      = author,
            Publisher   = publisher,
            Isbn        = isbn,
            Pages       = pages,
            Language    = language,
            ReleaseYear = releaseYear,
            CoverPath   = coverPath,
            AddedAt     = DateTime.UtcNow
        };

        db.Books.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Books
            .Include(b => b.Genres)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Book>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.Books
            .Include(b => b.Genres)
            .OrderBy(b => b.Author)
            .ThenBy(b => b.Title)
            .ToListAsync();
    }

    public async Task UpdateAsync(Book item)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.Books.Update(item);
        await db.SaveChangesAsync();
    }
}