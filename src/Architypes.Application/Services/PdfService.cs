using Architypes.Application.Interfaces;
using Architypes.Core.Enums;
using Architypes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Architypes.Application.Services;

public class PdfService : IPdfService
{
    private readonly ArchitypesDbContext _context;
    private readonly IAssessmentService _assessmentService;

    public PdfService(ArchitypesDbContext context, IAssessmentService assessmentService)
    {
        _context = context;
        _assessmentService = assessmentService;

        // Configure QuestPDF license (Community)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateArchetypeReportAsync(Guid sessionId)
    {
        var assessment = await _context.Assessments
            .Include(a => a.Result)
                .ThenInclude(r => r!.PrimaryArchetype)
            .Include(a => a.Result)
                .ThenInclude(r => r!.SecondaryArchetype)
            .Include(a => a.Result)
                .ThenInclude(r => r!.ShadowArchetype)
            .FirstOrDefaultAsync(a => a.SessionId == sessionId);

        if (assessment == null || assessment.Result == null)
        {
            throw new ArgumentException("Assessment not found or not completed");
        }

        if (!assessment.HasPurchasedFullReport)
        {
            throw new UnauthorizedAccessException("Full report not purchased");
        }

        var scores = await _assessmentService.GetArchetypeScoresAsync(assessment.Id);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header()
                    .Height(100)
                    .Background(Colors.Blue.Lighten3)
                    .Padding(20)
                    .Column(column =>
                    {
                        column.Item().Text("Your Archetype Profile")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.White);

                        column.Item().Text($"Generated: {DateTime.Now:MMMM dd, yyyy}")
                            .FontSize(10)
                            .FontColor(Colors.White);
                    });

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        // Primary Archetype Section
                        var primaryName = assessment.Gender == Gender.Male
                            ? assessment.Result.PrimaryArchetype.MaleName
                            : assessment.Result.PrimaryArchetype.FemaleName;

                        column.Item().PaddingBottom(10).Text("Your Primary Archetype")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().PaddingBottom(5).Text(primaryName)
                            .FontSize(20)
                            .Bold();

                        column.Item().PaddingBottom(10).Text(assessment.Result.PrimaryArchetype.CoreDrive)
                            .Italic()
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingBottom(10).Text(assessment.Result.PrimaryArchetype.DetailedCharacteristics)
                            .LineHeight(1.5f);

                        column.Item().PaddingBottom(5).Text("Strengths:")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Green.Darken2);

                        column.Item().PaddingBottom(10).Text(assessment.Result.PrimaryArchetype.Strengths)
                            .LineHeight(1.5f);

                        column.Item().PaddingBottom(5).Text("Blindspots:")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Orange.Darken2);

                        column.Item().PaddingBottom(10).Text(assessment.Result.PrimaryArchetype.Blindspots)
                            .LineHeight(1.5f);

                        column.Item().PaddingBottom(5).Text("In Business:")
                            .FontSize(14)
                            .Bold()
                            .FontColor(Colors.Blue.Darken1);

                        column.Item().PaddingBottom(20).Text(assessment.Result.PrimaryArchetype.InBusiness)
                            .LineHeight(1.5f);

                        // Secondary Archetype (if exists)
                        if (assessment.Result.SecondaryArchetype != null)
                        {
                            var secondaryName = assessment.Gender == Gender.Male
                                ? assessment.Result.SecondaryArchetype.MaleName
                                : assessment.Result.SecondaryArchetype.FemaleName;

                            column.Item().PageBreak();

                            column.Item().PaddingBottom(10).Text("Your Secondary Archetype")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Purple.Darken2);

                            column.Item().PaddingBottom(5).Text(secondaryName)
                                .FontSize(20)
                                .Bold();

                            column.Item().PaddingBottom(10).Text(assessment.Result.SecondaryArchetype.CoreDrive)
                                .Italic()
                                .FontColor(Colors.Grey.Darken1);

                            column.Item().PaddingBottom(10).Text(assessment.Result.SecondaryArchetype.DetailedCharacteristics)
                                .LineHeight(1.5f);

                            column.Item().PaddingBottom(5).Text("Strengths:")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Green.Darken2);

                            column.Item().PaddingBottom(10).Text(assessment.Result.SecondaryArchetype.Strengths)
                                .LineHeight(1.5f);

                            column.Item().PaddingBottom(5).Text("Blindspots:")
                                .FontSize(14)
                                .Bold()
                                .FontColor(Colors.Orange.Darken2);

                            column.Item().PaddingBottom(20).Text(assessment.Result.SecondaryArchetype.Blindspots)
                                .LineHeight(1.5f);
                        }

                        // Full Rankings Table
                        column.Item().PageBreak();

                        column.Item().PaddingBottom(10).Text("Complete Archetype Rankings")
                            .FontSize(18)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Lighten2)
                                    .Padding(5).Text("Rank").Bold();

                                header.Cell().Background(Colors.Blue.Lighten2)
                                    .Padding(5).Text("Archetype").Bold();

                                header.Cell().Background(Colors.Blue.Lighten2)
                                    .Padding(5).Text("Score").Bold();
                            });

                            // Rows
                            foreach (var score in scores)
                            {
                                var bgColor = score.Rank % 2 == 0 ? Colors.Grey.Lighten3 : Colors.White;

                                table.Cell().Background(bgColor)
                                    .Padding(5).Text($"#{score.Rank}");

                                table.Cell().Background(bgColor)
                                    .Padding(5).Text(score.DisplayName);

                                table.Cell().Background(bgColor)
                                    .Padding(5).Text($"{score.AverageScore:F2}");
                            }
                        });

                        // Shadow Archetype
                        if (assessment.Result.ShadowArchetype != null)
                        {
                            column.Item().PageBreak();

                            var shadowName = assessment.Gender == Gender.Male
                                ? assessment.Result.ShadowArchetype.MaleName
                                : assessment.Result.ShadowArchetype.FemaleName;

                            column.Item().PaddingBottom(10).Text("Your Shadow Archetype")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Orange.Darken3);

                            column.Item().PaddingBottom(5).Text(shadowName)
                                .FontSize(20)
                                .Bold();

                            column.Item().PaddingBottom(10).Text("Growth Opportunity")
                                .Italic()
                                .FontColor(Colors.Grey.Darken1);

                            column.Item().PaddingBottom(10).Text("This archetype represents your least expressed pattern. Developing this archetype can provide balance and open new possibilities for growth.")
                                .LineHeight(1.5f);

                            column.Item().PaddingBottom(10).Text(assessment.Result.ShadowArchetype.DetailedCharacteristics)
                                .LineHeight(1.5f);
                        }
                    });

                page.Footer()
                    .Height(50)
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated by Architypes • ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        });

        return document.GeneratePdf();
    }
}
