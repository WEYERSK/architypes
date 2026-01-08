namespace Architypes.Core.Entities;

public class AssessmentResult
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public required string TopArchetypeScores { get; set; } // JSON: [{ArchetypeId, Score, Rank}]
    public int PrimaryArchetypeId { get; set; }
    public int? SecondaryArchetypeId { get; set; }
    public int? ShadowArchetypeId { get; set; }
    public DateTime CalculatedAt { get; set; }

    public Assessment Assessment { get; set; } = null!;
    public Archetype PrimaryArchetype { get; set; } = null!;
    public Archetype? SecondaryArchetype { get; set; }
    public Archetype? ShadowArchetype { get; set; }
}
