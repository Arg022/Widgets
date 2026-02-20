namespace Widgets.Data.Models;

public class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public bool IsArchived { get; set; }

    public KanbanBoard KanbanBoard { get; set; } = null!;

    public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
    public ICollection<Folder> Folders { get; set; } = new List<Folder>();
}