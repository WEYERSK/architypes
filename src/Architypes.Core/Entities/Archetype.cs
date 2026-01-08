using Architypes.Core.Enums;

namespace Architypes.Core.Entities;

public class Archetype
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string MaleName { get; set; }
    public required string FemaleName { get; set; }
    public required string CoreDrive { get; set; }
    public required string Strengths { get; set; }
    public required string Shadow { get; set; }
    public required string InBusiness { get; set; }
    public required string FreeTeaser { get; set; }
    public required string DetailedCharacteristics { get; set; }
    public required string Blindspots { get; set; }
    public required string InteractionPatterns { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
