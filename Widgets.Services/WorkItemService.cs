using Microsoft.EntityFrameworkCore;
using Widgets.Data;
using Widgets.Data.Models;

namespace Widgets.Services;

public class WorkItemService
{
    private readonly AppDbContext _context;

    public WorkItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkItem> CreateAsync(
        Guid projectId,
        Guid columnId,
        string title,
        string type = "Task",
        int priority = 0,
        double? estimatedHours = null,
        Guid? sprintId = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? parentId = null)
    {
        var maxOrder = await _context.WorkItems
            .Where(w => w.ColumnId == columnId)
            .Select(w => (int?)w.OrderInColumn)
            .MaxAsync() ?? 0;

        var item = new WorkItem
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ColumnId = columnId,
            Title = title,
            WorkItemType = type,
            Priority = priority,
            EstimatedHours = estimatedHours,
            SprintId = sprintId,
            StartDate = startDate,
            EndDate = endDate,
            ParentId = parentId,
            CreatedAt = DateTime.UtcNow,
            OrderInColumn = maxOrder + 1
        };

        _context.WorkItems.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    public async Task MoveToColumnAsync(Guid workItemId, Guid newColumnId)
    {
        var item = await _context.WorkItems.FindAsync(workItemId);
        if (item == null) return;

        var maxOrder = await _context.WorkItems
            .Where(w => w.ColumnId == newColumnId)
            .Select(w => (int?)w.OrderInColumn)
            .MaxAsync() ?? 0;

        item.ColumnId = newColumnId;
        item.OrderInColumn = maxOrder + 1;

        await _context.SaveChangesAsync();
    }

    public async Task AssignToSprintAsync(Guid workItemId, Guid? sprintId)
    {
        var item = await _context.WorkItems.FindAsync(workItemId);
        if (item == null) return;

        item.SprintId = sprintId;
        await _context.SaveChangesAsync();
    }

    public async Task<List<WorkItem>> GetByProjectAsync(Guid projectId)
    {
        return await _context.WorkItems
            .Include(w => w.Column)
            .Where(w => w.ProjectId == projectId)
            .OrderBy(w => w.Column.Order)
            .ThenBy(w => w.OrderInColumn)
            .ToListAsync();
    }

    public async Task<List<WorkItem>> GetBySprintAsync(Guid sprintId)
    {
        return await _context.WorkItems
            .Where(w => w.SprintId == sprintId)
            .ToListAsync();
    }
}