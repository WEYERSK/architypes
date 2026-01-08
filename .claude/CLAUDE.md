# Kent's Global Development Standards

This file contains global standards and preferences for all projects.

## Technology Stack

**Runtime & Framework:**
- .NET 8+ (LTS)
- ASP.NET Core Web API / Minimal APIs
- Entity Framework Core 8+
- SQL Server

**Frontend:**
- Blazor Server/WebAssembly → MudBlazor components
- Razor Pages/MVC → Bootstrap 5
- No Next.js, Node.js, or JavaScript frameworks

**Authentication:**
- ASP.NET Core Identity
- Duende IdentityServer (when needed)

## Documentation Tools

### Context7 (Primary)
Use for all .NET and library documentation. Call `resolve-library-id` first, then `get-library-docs`.

**Key libraries:**
- `/dotnet/aspnetcore` - ASP.NET Core
- `/websites/learn_microsoft-en-us-aspnet-core` - Microsoft Docs
- `/dotnet/efcore` - Entity Framework Core
- `/websites/developers_payfast_co_za` - PayFast
- `/websites/paystack` - Paystack

### Jina (Secondary)
Use for arbitrary URLs not in Context7.

```
API_KEY: jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY
```

**Usage:**
```bash
# Search
curl "https://s.jina.ai/?q=SEARCH_TERM+llm.txt" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"

# Fetch URL
curl "https://r.jina.ai/URL_HERE" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

## Payment Gateways (South Africa)

Stripe is NOT available in South Africa. Use these alternatives:

| Gateway | Best For | .NET SDK | NuGet Package |
|---------|----------|----------|---------------|
| **PayFast** (default) | General SA businesses | ✅ Official | `PayFast`, `PayFast.AspNetCore` |
| **Paystack** | Multi-country Africa | ✅ Community | `Paystack.Net` |
| **Stitch** | Enterprise/Open Banking | ❌ Custom | GraphQL via HttpClient |

**Default choice: PayFast** unless project requires multi-country or enterprise features.

## Code Quality Standards

### Required
- C# nullable reference types enabled
- .NET Analyzers configured
- EditorConfig for formatting
- Async/await throughout
- FluentValidation for input validation
- No hardcoded values - use IConfiguration/IOptions

### Project Structure
```
MySolution/
├── src/
│   ├── MyApp.Api/              # Web API
│   ├── MyApp.Web/              # Blazor/Razor frontend
│   ├── MyApp.Core/             # Domain models, interfaces
│   ├── MyApp.Infrastructure/   # EF Core, external services
│   └── MyApp.Application/      # Business logic
├── tests/
│   ├── MyApp.UnitTests/
│   ├── MyApp.IntegrationTests/
│   └── MyApp.E2ETests/
└── MySolution.sln
```

### Security
- User Secrets for local dev (`dotnet user-secrets`)
- Environment variables for production
- No secrets in appsettings.json committed to git
- HTTPS enforced
- Payment webhook signature validation

### Testing
- xUnit + Moq for unit tests
- WebApplicationFactory for integration tests
- Playwright for E2E tests
- Coverage > 80%

## Critical Rules

### NO HARDCODES, NO PLACEHOLDERS
- Never hardcode API keys, prices, or config
- Never use "TODO" or "Coming soon" placeholders
- Complete features fully before moving on

### When Blocked
- Don't guess or use workarounds
- Ask for clarification
- Check documentation first (Context7 → Jina)

## Common Commands

```bash
# New solution
dotnet new sln -n MySolution
dotnet new webapi -n MyApp.Api
dotnet new blazorserver -n MyApp.Web

# Packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package PayFast
dotnet add package PayFast.AspNetCore
dotnet add package MudBlazor
dotnet add package FluentValidation.AspNetCore

# EF Core
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run
dotnet watch --project MyApp.Web
```
