using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Widgets.Data;

public class AppDbContextDesignFactory 
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Percorso semplice per migration
        optionsBuilder.UseSqlite("Data Source=widgets_design.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}