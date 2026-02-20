namespace Widgets.Data.Models;

public class Folder
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }

    public Guid? ParentFolderId { get; set; }
    public Folder? ParentFolder { get; set; }

    public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int Order { get; set; }
}