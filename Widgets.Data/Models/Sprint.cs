namespace Widgets.Data.Models;

public class Sprint
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsClosed { get; set; }

    public int Order { get; set; }

    public ICollection<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
}