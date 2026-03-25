using Microsoft.EntityFrameworkCore;
using Widgets.Data.Models;

namespace Widgets.Data;

public class AppDbContext : DbContext
{
    public DbSet<Item>      Items      => Set<Item>();
    public DbSet<Videogame> Videogames => Set<Videogame>();
    public DbSet<VideoWork> VideoWorks => Set<VideoWork>();
    public DbSet<Album>     Albums     => Set<Album>();
    public DbSet<Book>      Books      => Set<Book>();
    public DbSet<Manga>     Mangas     => Set<Manga>();
    public DbSet<Genre>     Genres     => Set<Genre>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Item>().UseTpcMappingStrategy();
        //modelBuilder.Entity<Item>().UseTphMappingStrategy();
        modelBuilder.Entity<Item>().UseTptMappingStrategy();
        
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Status)
                  .HasDefaultValue("Wishlist");

            entity.Property(i => i.AddedAt)
                  .HasDefaultValueSql("strftime('%Y-%m-%dT%H:%M:%S','now')");

            entity.ToTable(t => t.HasCheckConstraint("CK_Item_Rating",
                "Rating IS NULL OR (Rating >= 0 AND Rating <= 10)"));

            // N:M con Genre tramite tabella junction ItemGenres
            entity.HasMany(i => i.Genres)
                  .WithMany(g => g.Items)
                  .UsingEntity(j => j.ToTable("ItemGenres"));
        });
        
        modelBuilder.Entity<Videogame>(entity =>
        {
            entity.Property(v => v.HoursPlayed).HasDefaultValue(0.0);
            entity.Property(v => v.Completion).HasDefaultValue(0);

            entity.ToTable(t => t.HasCheckConstraint("CK_Game_Completion",
                "Completion >= 0 AND Completion <= 100"));
        });
        
        modelBuilder.Entity<VideoWork>(entity =>
        {
            entity.Property(v => v.WatchedEpisodes).HasDefaultValue(0);
        });
        
        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(a => a.Format).HasDefaultValue("Digital");

            entity.ToTable(t => t.HasCheckConstraint("CK_Album_Format",
                "Format IN ('Digital','Vinyl','CD','Cassette')"));
        });
        
        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.CurrentPage).HasDefaultValue(0);

            entity.HasIndex(b => b.Isbn).IsUnique();
        });
        
        modelBuilder.Entity<Manga>(entity =>
        {
            entity.Property(m => m.Origin).HasDefaultValue("Manga");
            entity.Property(m => m.ReadVolumes).HasDefaultValue(0);
            entity.Property(m => m.ReadChapters).HasDefaultValue(0);
            entity.Property(m => m.IsCompleted).HasDefaultValue(false);

            entity.ToTable(t => t.HasCheckConstraint("CK_Manga_Origin",
                "Origin IN ('Manga','Manhwa','Manhua','Western', 'Novel')"));
        });
        
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.Id);
            entity.HasIndex(g => g.Name).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}