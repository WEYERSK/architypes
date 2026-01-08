using Architypes.Application.Models;
using Architypes.Core.Entities;
using Architypes.Core.Enums;

namespace Architypes.Application.Interfaces;

public interface IAssessmentService
{
    Task<Assessment> CreateAssessmentAsync(Guid sessionId, Gender gender);
    Task<Assessment?> GetAssessmentBySessionIdAsync(Guid sessionId);
    Task SaveAnswerAsync(int assessmentId, int questionId, int rating);
    Task<AssessmentResult> CalculateResultsAsync(int assessmentId);
    Task<List<ArchetypeScore>> GetArchetypeScoresAsync(int assessmentId);
    Task MarkAsPaidAsync(int assessmentId, string paymentReference);
    Task<List<Question>> GetAllQuestionsAsync();
}
