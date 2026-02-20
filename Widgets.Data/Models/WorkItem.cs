namespace Widgets.Data.Models;

public class WorkItem
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public Guid ColumnId { get; set; }
    public KanbanColumn Column { get; set; } = null!;

    public Guid? SprintId { get; set; }
    public Sprint? Sprint { get; set; }

    public Guid? ParentId { get; set; }
    public WorkItem? Parent { get; set; }
    public ICollection<WorkItem> Children { get; set; } = new List<WorkItem>();

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public string WorkItemType { get; set; } = "Task";

    public int Priority { get; set; }
    public int ImportanceScore { get; set; }

    public double? EstimatedHours { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public int OrderInColumn { get; set; }

    public ICollection<Note> Notes { get; set; } = new List<Note>();
}