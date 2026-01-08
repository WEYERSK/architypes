using Architypes.Core.Enums;

namespace Architypes.Core.Entities;

public class Assessment
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public Gender Gender { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsCompleted { get; set; }
    public bool HasPurchasedFullReport { get; set; }
    public string? PaymentReference { get; set; }
    public string? Email { get; set; }

    public ICollection<AssessmentAnswer> Answers { get; set; } = new List<AssessmentAnswer>();
    public AssessmentResult? Result { get; set; }
}
