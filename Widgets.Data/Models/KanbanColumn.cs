namespace Widgets.Data.Models;

public class KanbanColumn
{
    public Guid Id { get; set; }

    public Guid BoardId { get; set; }
    public KanbanBoard Board { get; set; } = null!;

    public string Name { get; set; } = null!;
    public int Order { get; set; }

    public int? WipLimit { get; set; }

    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}