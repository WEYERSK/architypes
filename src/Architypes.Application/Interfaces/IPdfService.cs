namespace Architypes.Application.Interfaces;

public interface IPdfService
{
    Task<byte[]> GenerateArchetypeReportAsync(Guid sessionId);
}
