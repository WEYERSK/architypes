using Architypes.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Architypes.Web.Pages;

public class DownloadReportModel : PageModel
{
    private readonly IPdfService _pdfService;
    private readonly ILogger<DownloadReportModel> _logger;

    public DownloadReportModel(IPdfService pdfService, ILogger<DownloadReportModel> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    [FromRoute]
    public Guid SessionId { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var pdfBytes = await _pdfService.GenerateArchetypeReportAsync(SessionId);

            return File(pdfBytes, "application/pdf", $"ArchetypeReport_{SessionId:N}.pdf");
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning("Unauthorized PDF download attempt for session {SessionId}", SessionId);
            return RedirectToPage("/ResultsPage", new { SessionId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for session {SessionId}", SessionId);
            return RedirectToPage("/ResultsPage", new { SessionId });
        }
    }
}
