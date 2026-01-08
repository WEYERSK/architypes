# Architypes - Archetype Assessment Platform

A comprehensive web application for discovering and analyzing personality archetypes. Users complete a 36-question assessment and receive personalized archetype analysis with optional paid full reports.

## Features

- **Gender-Specific Assessments**: Male and Female versions with tailored archetype names
- **36-Question Assessment**: 3 questions per archetype with 1-5 star rating system
- **Free Results**: Top archetype with teaser description
- **Paid Full Reports** (R49):
  - Complete ranking of all 12 archetypes
  - Primary, Secondary, and Shadow archetype analysis
  - Detailed strengths, blindspots, and business applications
  - Professional PDF report download
- **Secure Payments**: PayFast integration for South African users
- **Anonymous Assessment**: No account required - session-based tracking

## Architecture

Built with **.NET 8** and **Clean Architecture**:

```
Architypes/
├── src/
│   ├── Architypes.Web/              # Blazor Server UI (MudBlazor)
│   ├── Architypes.Application/      # Business logic & services
│   ├── Architypes.Infrastructure/   # EF Core, database, data access
│   └── Architypes.Core/             # Domain entities & interfaces
└── tests/
    ├── Architypes.UnitTests/
    └── Architypes.IntegrationTests/
```

## Technology Stack

- **Frontend**: Blazor Server + MudBlazor
- **Backend**: ASP.NET Core 8
- **Database**: SQL Server with Entity Framework Core 8
- **Payment**: PayFast (South Africa)
- **PDF Generation**: QuestPDF

## The 12 Archetypes

1. **King/Queen** - Order, responsibility, sovereignty
2. **Warrior/Huntress** - Discipline, courage, boundaries
3. **Magician/Mystic** - Transformation, insight, mastery
4. **Lover** - Connection, passion, aliveness
5. **Sage** - Truth, understanding, wisdom
6. **Explorer/Wild Woman** - Freedom, discovery, autonomy
7. **Creator/Creatrix** - Innovation, expression
8. **Hero** - Mastery, achievement, proving worth
9. **Rebel/Outlaw** - Liberation, revolution
10. **Jester/Fool** - Joy, presence, truth through humor
11. **Caregiver/Healer** - Service, compassion, nurturing
12. **Father/Mother** - Protection, guidance, provision

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or full instance)
- PayFast account (for production payments)

## Setup Instructions

### 1. Database Setup

The application uses SQL Server LocalDB by default. The database will be created automatically on first run.

```bash
# Run migrations (if needed)
dotnet ef database update --project src/Architypes.Infrastructure --startup-project src/Architypes.Web
```

The database will be seeded with:
- All 12 archetypes with complete data
- 36 questions (3 per archetype)

### 2. PayFast Configuration

For production, you'll need PayFast merchant credentials:

```bash
# Initialize user secrets
cd src/Architypes.Web
dotnet user-secrets init

# Set PayFast credentials
dotnet user-secrets set "PayFast:MerchantId" "YOUR_MERCHANT_ID"
dotnet user-secrets set "PayFast:MerchantKey" "YOUR_MERCHANT_KEY"
dotnet user-secrets set "PayFast:Passphrase" "YOUR_PASSPHRASE"
```

**For development/testing**, the sandbox credentials in `appsettings.json` work with empty values.

### 3. Application Settings

Edit `src/Architypes.Web/appsettings.json` if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Architypes;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "PayFast": {
    "ProcessUrl": "https://sandbox.payfast.co.za/eng/process",
    "ValidateUrl": "https://sandbox.payfast.co.za/eng/query/validate",
    "ReturnUrl": "https://localhost:5001/payment/return",
    "CancelUrl": "https://localhost:5001/payment/cancel",
    "NotifyUrl": "https://localhost:5001/payment/notify"
  },
  "Pricing": {
    "FullReportPrice": 49.00,
    "Currency": "ZAR"
  }
}
```

### 4. Build and Run

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run the application
dotnet run --project src/Architypes.Web
```

Navigate to `https://localhost:5001` or `http://localhost:5000`

## User Flow

1. **Landing Page** → Select Male or Female version
2. **Assessment** → Answer 36 questions (1-5 star rating)
3. **Free Results** → View top archetype with teaser
4. **Paywall** → Option to purchase full report (R49)
5. **Payment** → Secure PayFast checkout
6. **Full Results** → Complete analysis + PDF download

## Development

### Project Structure

- **Architypes.Core**: Domain entities, enums, configuration models
- **Architypes.Application**: Services, interfaces, business logic
- **Architypes.Infrastructure**: DbContext, migrations, data seeding
- **Architypes.Web**: Blazor pages, components, UI

### Key Services

- `IAssessmentService`: Assessment creation, scoring, results calculation
- `IPaymentService`: PayFast payment generation and validation
- `IPdfService`: QuestPDF report generation

### Database Schema

- **Archetypes**: 12 archetypes with characteristics, strengths, shadows
- **Questions**: 36 questions (3 per archetype)
- **Assessments**: User assessment sessions
- **AssessmentAnswers**: User responses (1-5 ratings)
- **AssessmentResults**: Calculated scores and rankings

## Deployment

### Production Checklist

1. Update `appsettings.Production.json`:
   - Production SQL Server connection string
   - Production PayFast URLs and credentials
   - Configure logging

2. Set environment variables:
   ```bash
   export ASPNETCORE_ENVIRONMENT=Production
   ```

3. PayFast webhook URLs must be publicly accessible:
   - ReturnUrl: `https://yourdomain.com/payment/return`
   - NotifyUrl: `https://yourdomain.com/payment/notify`
   - CancelUrl: `https://yourdomain.com/payment/cancel`

4. Configure HTTPS and SSL certificates

5. Run database migrations:
   ```bash
   dotnet ef database update --project src/Architypes.Infrastructure --startup-project src/Architypes.Web
   ```

## Testing PayFast Integration

PayFast Sandbox credentials:
- Leave MerchantId, MerchantKey empty in development
- Use test card numbers from PayFast documentation
- Monitor PayFast sandbox dashboard for notifications

## Pricing

- **Free**: Top archetype + teaser
- **Full Report**: R49.00 (ZAR)
  - All 12 archetypes ranked
  - Detailed analysis
  - PDF download

## License

Proprietary - All rights reserved

## Support

For issues or questions, contact [your contact email]
