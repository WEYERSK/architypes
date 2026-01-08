using System.Text.Json;
using Architypes.Application.Interfaces;
using Architypes.Application.Models;
using Architypes.Core.Entities;
using Architypes.Core.Enums;
using Architypes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Architypes.Application.Services;

public class AssessmentService : IAssessmentService
{
    private readonly ArchitypesDbContext _context;

    public AssessmentService(ArchitypesDbContext context)
    {
        _context = context;
    }

    public async Task<Assessment> CreateAssessmentAsync(Guid sessionId, Gender gender)
    {
        // Check if assessment already exists for this session
        var existing = await _context.Assessments
            .FirstOrDefaultAsync(a => a.SessionId == sessionId);

        if (existing != null)
        {
            return existing;
        }

        var assessment = new Assessment
        {
            SessionId = sessionId,
            Gender = gender,
            CreatedAt = DateTime.UtcNow,
            IsCompleted = false,
            HasPurchasedFullReport = false
        };

        _context.Assessments.Add(assessment);
        await _context.SaveChangesAsync();

        return assessment;
    }

    public async Task<Assessment?> GetAssessmentBySessionIdAsync(Guid sessionId)
    {
        return await _context.Assessments
            .Include(a => a.Answers)
                .ThenInclude(ans => ans.Question)
            .Include(a => a.Result)
                .ThenInclude(r => r!.PrimaryArchetype)
            .Include(a => a.Result)
                .ThenInclude(r => r!.SecondaryArchetype)
            .Include(a => a.Result)
                .ThenInclude(r => r!.ShadowArchetype)
            .FirstOrDefaultAsync(a => a.SessionId == sessionId);
    }

    public async Task SaveAnswerAsync(int assessmentId, int questionId, int rating)
    {
        if (rating < 1 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5");
        }

        var existingAnswer = await _context.AssessmentAnswers
            .FirstOrDefaultAsync(a => a.AssessmentId == assessmentId && a.QuestionId == questionId);

        if (existingAnswer != null)
        {
            existingAnswer.Rating = rating;
        }
        else
        {
            var answer = new AssessmentAnswer
            {
                AssessmentId = assessmentId,
                QuestionId = questionId,
                Rating = rating
            };
            _context.AssessmentAnswers.Add(answer);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<AssessmentResult> CalculateResultsAsync(int assessmentId)
    {
        var assessment = await _context.Assessments
            .Include(a => a.Answers)
                .ThenInclude(ans => ans.Question)
                    .ThenInclude(q => q.Archetype)
            .FirstOrDefaultAsync(a => a.Id == assessmentId);

        if (assessment == null)
        {
            throw new ArgumentException("Assessment not found");
        }

        if (assessment.Answers.Count < 36)
        {
            throw new InvalidOperationException("All questions must be answered before calculating results");
        }

        // Calculate average score for each archetype
        var archetypeScores = assessment.Answers
            .GroupBy(a => a.Question.ArchetypeId)
            .Select(g => new
            {
                ArchetypeId = g.Key,
                Archetype = g.First().Question.Archetype,
                AverageScore = g.Average(a => a.Rating)
            })
            .OrderByDescending(x => x.AverageScore)
            .ToList();

        var rankedScores = archetypeScores
            .Select((x, index) => new ArchetypeScore
            {
                ArchetypeId = x.ArchetypeId,
                ArchetypeName = x.Archetype.Name,
                DisplayName = assessment.Gender == Gender.Male ? x.Archetype.MaleName : x.Archetype.FemaleName,
                AverageScore = x.AverageScore,
                Rank = index + 1
            })
            .ToList();

        // Determine primary, secondary, and shadow archetypes
        var primaryArchetype = archetypeScores[0];
        var secondaryArchetype = archetypeScores.Count > 1 ? archetypeScores[1] : null;
        var shadowArchetype = archetypeScores.LastOrDefault(); // Lowest score

        // Check if result already exists
        var existingResult = await _context.AssessmentResults
            .FirstOrDefaultAsync(r => r.AssessmentId == assessmentId);

        if (existingResult != null)
        {
            existingResult.TopArchetypeScores = JsonSerializer.Serialize(rankedScores);
            existingResult.PrimaryArchetypeId = primaryArchetype.ArchetypeId;
            existingResult.SecondaryArchetypeId = secondaryArchetype?.ArchetypeId;
            existingResult.ShadowArchetypeId = shadowArchetype?.ArchetypeId;
            existingResult.CalculatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingResult;
        }

        var result = new AssessmentResult
        {
            AssessmentId = assessmentId,
            TopArchetypeScores = JsonSerializer.Serialize(rankedScores),
            PrimaryArchetypeId = primaryArchetype.ArchetypeId,
            SecondaryArchetypeId = secondaryArchetype?.ArchetypeId,
            ShadowArchetypeId = shadowArchetype?.ArchetypeId,
            CalculatedAt = DateTime.UtcNow
        };

        _context.AssessmentResults.Add(result);

        // Mark assessment as completed
        assessment.IsCompleted = true;
        assessment.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return result;
    }

    public async Task<List<ArchetypeScore>> GetArchetypeScoresAsync(int assessmentId)
    {
        var result = await _context.AssessmentResults
            .FirstOrDefaultAsync(r => r.AssessmentId == assessmentId);

        if (result == null)
        {
            return new List<ArchetypeScore>();
        }

        var scores = JsonSerializer.Deserialize<List<ArchetypeScore>>(result.TopArchetypeScores);
        return scores ?? new List<ArchetypeScore>();
    }

    public async Task MarkAsPaidAsync(int assessmentId, string paymentReference)
    {
        var assessment = await _context.Assessments.FindAsync(assessmentId);
        if (assessment != null)
        {
            assessment.HasPurchasedFullReport = true;
            assessment.PaymentReference = paymentReference;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _context.Questions
            .Include(q => q.Archetype)
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync();
    }
}
