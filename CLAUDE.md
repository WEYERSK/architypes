# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Architypes is an archetype assessment platform built with .NET 8 and Blazor Server. Users complete a 36-question assessment (3 questions per archetype) and receive a free result showing their top archetype. A paid full report (R49) unlocks complete analysis with PDF download via PayFast payment integration.

## Essential Commands

```bash
# Build and run
dotnet restore
dotnet build
dotnet run --project src/Architypes.Web

# Development with hot reload
dotnet watch --project src/Architypes.Web

# Database migrations
dotnet ef migrations add MigrationName --project src/Architypes.Infrastructure --startup-project src/Architypes.Web
dotnet ef database update --project src/Architypes.Infrastructure --startup-project src/Architypes.Web

# PayFast credentials (development)
cd src/Architypes.Web
dotnet user-secrets init
dotnet user-secrets set "PayFast:MerchantId" "YOUR_MERCHANT_ID"
dotnet user-secrets set "PayFast:MerchantKey" "YOUR_MERCHANT_KEY"
dotnet user-secrets set "PayFast:Passphrase" "YOUR_PASSPHRASE"
```

## Architecture

**Clean Architecture with 4 layers:**

1. **Architypes.Core** (Domain Layer)
   - Entities: Archetype, Assessment, AssessmentAnswer, AssessmentResult, Question
   - Enums: Gender (Male/Female for archetype naming)
   - Configuration: PayFastSettings, PricingSettings
   - No dependencies on other layers

2. **Architypes.Application** (Business Logic)
   - Services: AssessmentService, PayFastPaymentService, PdfService
   - Interfaces: IAssessmentService, IPaymentService, IPdfService
   - Models: ArchetypeScore (used for ranking display)
   - Depends on: Core, Infrastructure (for DbContext injection)

3. **Architypes.Infrastructure** (Data Access)
   - ArchitypesDbContext with EF Core 8
   - DbSeeder: Seeds 12 archetypes + 36 questions on startup
   - Migrations assembly specified in Program.cs
   - Depends on: Core

4. **Architypes.Web** (Presentation)
   - Blazor Server with MudBlazor components
   - Session-based tracking using browser sessionStorage (anonymous users)
   - Pages: Index, AssessmentPage, ResultsPage, PaymentPage, PaymentReturn, PaymentCancel
   - Razor Pages: DownloadReport.cshtml, PaymentNotify.cshtml (for PayFast webhook)
   - Depends on: Application, Infrastructure, Core

## Critical Architecture Details

### Session-Based User Tracking

Users are tracked anonymously via GUID stored in browser sessionStorage:
- SessionId generated on Index.razor when user selects gender
- Stored in browser: `sessionStorage.setItem("assessmentSessionId", guid)`
- Assessment.SessionId links all user data (Assessment → AssessmentAnswers → AssessmentResult)
- No authentication required - session persists 30 days via `builder.Services.AddSession()`

### Scoring Algorithm

Located in `AssessmentService.CalculateResultsAsync()`:
1. Groups 36 answers by ArchetypeId (3 questions per archetype)
2. Calculates average rating (1-5) for each archetype
3. Ranks all 12 archetypes by average score (descending)
4. Stores as JSON in AssessmentResult.TopArchetypeScores
5. Sets PrimaryArchetypeId (rank 1), SecondaryArchetypeId (rank 2), ShadowArchetypeId (rank 12)

### Gender-Specific Display

Each Archetype has both `MaleName` and `FemaleName`:
- Assessment.Gender determines which name to display
- UI conditionally renders: `assessment.Gender == Gender.Male ? archetype.MaleName : archetype.FemaleName`
- Examples: "The King" / "The Queen", "The Warrior" / "The Huntress"

### Paywall Implementation

ResultsPage.razor checks `assessment.HasPurchasedFullReport`:
- **Free**: Shows primary archetype with `FreeTeaser` property only
- **Paid**: Shows full rankings table, all archetype details, PDF download button
- Payment flow: ResultsPage → PaymentPage → PayFast → PaymentReturn → ResultsPage (with full access)

### PayFast Integration

**Payment Flow:**
1. PaymentPage.razor generates HTML form with PayFastPaymentService
2. Form auto-submits to PayFast sandbox/production URL
3. User completes payment on PayFast site
4. PayFast sends webhook to PaymentNotify.cshtml.cs (ITN - Instant Transaction Notification)
5. PaymentNotify validates signature, updates Assessment.HasPurchasedFullReport = true
6. User returns to ResultsPage with full access

**Signature Validation:**
- MD5 hash of sorted parameters + optional passphrase
- Implementation in `PayFastPaymentService.GenerateSignature()` and `ValidatePaymentNotificationAsync()`
- CRITICAL: Always validate signature before processing payment

### Database Seeding

`DbSeeder.SeedDataAsync()` runs on application startup (Program.cs):
- Checks if Archetypes table has data
- Seeds 12 archetypes with complete data (FreeTeaser, DetailedCharacteristics, Strengths, Blindspots, etc.)
- Seeds 36 questions (3 per archetype) with DisplayOrder for consistent presentation
- Runs only once - idempotent by checking `context.Archetypes.AnyAsync()`

### PDF Generation

`PdfService` uses QuestPDF to generate reports:
- Called from DownloadReport.cshtml.cs endpoint: `/download-report/{sessionId}`
- Verifies `assessment.HasPurchasedFullReport == true` before generating
- Returns PDF as downloadable file: `File(pdfBytes, "application/pdf", filename)`
- Includes primary/secondary/shadow analysis, full rankings table, detailed characteristics

## Project-Specific Patterns

### Naming Conflict Resolution

Pages renamed to avoid conflicts with domain entities:
- `Assessment.razor` → `AssessmentPage.razor` (conflicted with Core.Entities.Assessment)
- `Results.razor` → `ResultsPage.razor` (conflicted with page component name)
- Use fully qualified types when needed: `Architypes.Core.Entities.Assessment`

### Configuration with IOptions

Settings loaded via IOptions pattern:
```csharp
builder.Services.Configure<PayFastSettings>(builder.Configuration.GetSection(PayFastSettings.SectionName));
builder.Services.Configure<PricingSettings>(builder.Configuration.GetSection(PricingSettings.SectionName));
```
Inject with: `@inject IOptions<PayFastSettings> PayFastSettings`
Access with: `PayFastSettings.Value.MerchantId`

### MudBlazor Component Usage

- **MudRating**: Used for 1-5 star question responses with `@bind-SelectedValue`
- **MudExpansionPanels**: Used for collapsible archetype details in paid results
- **MudTable**: Used for archetype rankings display
- **MudStack**: Replaced MudList to avoid generic type inference issues with MudBlazor 8.0

### Data Flow

1. **User starts assessment**: Index.razor → CreateAssessmentAsync(sessionId, gender)
2. **User answers questions**: AssessmentPage.razor → SaveAnswerAsync(assessmentId, questionId, rating)
3. **User submits**: AssessmentPage.razor → CalculateResultsAsync(assessmentId) → ResultsPage
4. **User pays**: ResultsPage → PaymentPage → PayFast → PaymentNotify webhook → ProcessSuccessfulPaymentAsync
5. **User downloads**: ResultsPage → DownloadReport endpoint → GenerateArchetypeReportAsync → PDF file

## Important Files

- **Program.cs**: Service registration, DbSeeder invocation, session configuration
- **DbSeeder.cs**: Complete archetype and question data (DO NOT MODIFY without backup)
- **AssessmentService.cs**: Core business logic for scoring algorithm
- **PayFastPaymentService.cs**: Payment signature generation and validation
- **ResultsPage.razor**: Paywall logic and conditional rendering based on payment status
- **PaymentNotify.cshtml.cs**: Webhook handler - must return HTTP 200 OK to PayFast

## Testing PayFast

Development uses PayFast sandbox mode:
- Set empty/null MerchantId and MerchantKey in appsettings.json
- ProcessUrl: `https://sandbox.payfast.co.za/eng/process`
- Webhook NotifyUrl must be publicly accessible (use ngrok for local testing)
- Test card numbers available in PayFast documentation

## Deployment Considerations

1. **Production PayFast URLs**: Update ProcessUrl, ValidateUrl in appsettings.Production.json
2. **Webhook accessibility**: NotifyUrl must be publicly reachable (not localhost)
3. **User Secrets**: Set PayFast credentials via environment variables or Azure Key Vault
4. **Database**: Run migrations before first deployment
5. **HTTPS**: Required for PayFast integration
