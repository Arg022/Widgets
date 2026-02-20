namespace Widgets.Data.Models;

public class KanbanBoard
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string Name { get; set; } = "Main Board";

    public ICollection<KanbanColumn> Columns { get; set; } = new List<KanbanColumn>();
}