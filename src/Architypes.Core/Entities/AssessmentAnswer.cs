namespace Architypes.Core.Entities;

public class AssessmentAnswer
{
    public int Id { get; set; }
    public int AssessmentId { get; set; }
    public int QuestionId { get; set; }
    public int Rating { get; set; } // 1-5 scale

    public Assessment Assessment { get; set; } = null!;
    public Question Question { get; set; } = null!;
}
