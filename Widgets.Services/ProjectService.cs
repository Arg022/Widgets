using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class ProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateProjectAsync(string name, string? description)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            IsArchived = false
        };

        var board = new KanbanBoard
        {
            Id = Guid.NewGuid(),
            Project = project,
            Name = "Main Board"
        };

        var todoColumn = new KanbanColumn
        {
            Id = Guid.NewGuid(),
            Board = board,
            Name = "Todo",
            Order = 0
        };

        var doingColumn = new KanbanColumn
        {
            Id = Guid.NewGuid(),
            Board = board,
            Name = "In Progress",
            Order = 1
        };

        var doneColumn = new KanbanColumn
        {
            Id = Guid.NewGuid(),
            Board = board,
            Name = "Done",
            Order = 2
        };

        _context.Projects.Add(project);
        _context.KanbanBoards.Add(board);
        _context.KanbanColumns.AddRange(todoColumn, doingColumn, doneColumn);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return project;
    }

    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.KanbanBoard)
            .ToListAsync();
    }
}