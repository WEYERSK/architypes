namespace Architypes.Application.Models;

public class ArchetypeScore
{
    public int ArchetypeId { get; set; }
    public required string ArchetypeName { get; set; }
    public required string DisplayName { get; set; } // Gender-specific name
    public double AverageScore { get; set; }
    public int Rank { get; set; }
}
