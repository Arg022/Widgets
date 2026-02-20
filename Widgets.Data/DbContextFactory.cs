using Microsoft.EntityFrameworkCore;

namespace Widgets.Data;

public static class DbContextFactory
{
    public static AppDbContext Create(string? dbPath = null)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Se non specificato, crea il DB nella cartella locale dell'utente
        if (string.IsNullOrEmpty(dbPath))
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(folder, "WidgetsApp");
            Directory.CreateDirectory(appFolder);

            dbPath = Path.Combine(appFolder, "widgets.db");
        }

        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new AppDbContext(optionsBuilder.Options);
    }
}