namespace Widgets.Data.Models;

public class Note
{
    public Guid Id { get; set; }

    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }

    public Guid? WorkItemId { get; set; }
    public WorkItem? WorkItem { get; set; }

    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;

    public int ImportanceScore { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}