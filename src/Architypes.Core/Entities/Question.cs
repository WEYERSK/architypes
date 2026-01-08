namespace Architypes.Core.Entities;

public class Question
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public int ArchetypeId { get; set; }
    public int DisplayOrder { get; set; }

    public Archetype Archetype { get; set; } = null!;
}
