using Microsoft.EntityFrameworkCore;
using Widgets.Data.Models;

namespace Widgets.Data;

public class AppDbContext : DbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<KanbanBoard> KanbanBoards => Set<KanbanBoard>();
    public DbSet<KanbanColumn> KanbanColumns => Set<KanbanColumn>();
    public DbSet<Sprint> Sprints => Set<Sprint>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<Folder> Folders => Set<Folder>();
    public DbSet<Note> Notes => Set<Note>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // =========================
        // Project
        // =========================
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasOne(p => p.KanbanBoard)
                  .WithOne(b => b.Project)
                  .HasForeignKey<KanbanBoard>(b => b.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Sprints)
                  .WithOne(s => s.Project)
                  .HasForeignKey(s => s.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.WorkItems)
                  .WithOne(w => w.Project)
                  .HasForeignKey(w => w.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Folders)
                  .WithOne(f => f.Project)
                  .HasForeignKey(f => f.ProjectId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // KanbanBoard
        // =========================
        modelBuilder.Entity<KanbanBoard>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasMany(b => b.Columns)
                  .WithOne(c => c.Board)
                  .HasForeignKey(c => c.BoardId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // KanbanColumn
        // =========================
        modelBuilder.Entity<KanbanColumn>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.WorkItems)
                  .WithOne(w => w.Column)
                  .HasForeignKey(w => w.ColumnId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================
        // Sprint
        // =========================
        modelBuilder.Entity<Sprint>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasMany(s => s.WorkItems)
                  .WithOne(w => w.Sprint)
                  .HasForeignKey(w => w.SprintId)
                  .OnDelete(DeleteBehavior.SetNull); // Sprint opzionale
        });

        // =========================
        // WorkItem
        // =========================
        modelBuilder.Entity<WorkItem>(entity =>
        {
            entity.HasKey(w => w.Id);

            // Parent-Children (self-reference)
            entity.HasOne(w => w.Parent)
                  .WithMany(w => w.Children)
                  .HasForeignKey(w => w.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Note collegate
            entity.HasMany(w => w.Notes)
                  .WithOne(n => n.WorkItem)
                  .HasForeignKey(n => n.WorkItemId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // Folder
        // =========================
        modelBuilder.Entity<Folder>(entity =>
        {
            entity.HasKey(f => f.Id);

            // Parent-Children folder
            entity.HasOne(f => f.ParentFolder)
                  .WithMany(f => f.SubFolders)
                  .HasForeignKey(f => f.ParentFolderId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Notes
            entity.HasMany(f => f.Notes)
                  .WithOne(n => n.Folder)
                  .HasForeignKey(n => n.FolderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // =========================
        // Note
        // =========================
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(n => n.Id);

            // Nota legata a WorkItem o Folder, nullable
            entity.HasCheckConstraint(
                "CK_Note_OnlyOneOwner",
                "(FolderId IS NOT NULL AND WorkItemId IS NULL) OR (FolderId IS NULL AND WorkItemId IS NOT NULL) OR (FolderId IS NULL AND WorkItemId IS NULL)"
            );
        });

        base.OnModelCreating(modelBuilder);
    }
}