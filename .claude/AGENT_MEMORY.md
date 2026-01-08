# Shared Agent Memory & Context

This file contains shared knowledge and context that ALL agents should be aware of.

## Documentation Access

### Context7 (Primary - for Library Documentation)

**ALL AGENTS** should use Context7 as the primary tool for .NET and library documentation. It has indexed, structured content optimised for LLM consumption.

**Key .NET Libraries in Context7:**
- `/dotnet/aspnetcore` - ASP.NET Core (10,049 snippets)
- `/websites/learn_microsoft-en-us-aspnet-core` - ASP.NET Core Docs (227,311 snippets)
- `/dotnet/efcore` - Entity Framework Core
- `/websites/developers_payfast_co_za` - PayFast Payment Gateway (33 snippets)
- `/websites/paystack` - Paystack Payment Gateway (873 snippets)

**Usage**: Call `resolve-library-id` first to find the correct library ID, then `get-library-docs` with topic focus.

### Jina (Secondary - for Arbitrary URLs)

Use Jina when you need to fetch specific URLs not indexed in Context7.

**Jina API Keys:**
```
API_KEY: jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY
```

**How to Use Jina:**

#### Search for Documentation (llms.txt)
```bash
curl "https://s.jina.ai/?q=PayFast+llm.txt" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

#### Fetch Specific Documentation Page
```bash
curl "https://r.jina.ai/https://developers.payfast.co.za/docs" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

### Common Documentation Sources

**Microsoft Learn (.NET):**
```bash
# ASP.NET Core Web API
curl "https://r.jina.ai/https://learn.microsoft.com/en-us/aspnet/core/web-api" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"

# Blazor
curl "https://r.jina.ai/https://learn.microsoft.com/en-us/aspnet/core/blazor" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"

# Entity Framework Core
curl "https://r.jina.ai/https://learn.microsoft.com/en-us/ef/core" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

**Payment Gateway Documentation:**
```bash
# PayFast Developer Docs
curl "https://r.jina.ai/https://developers.payfast.co.za/docs" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"

# Paystack Developer Docs
curl "https://r.jina.ai/https://paystack.com/docs/api" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"

# Stitch Developer Docs
curl "https://r.jina.ai/https://docs.stitch.money" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

**MudBlazor (Blazor Component Library):**
```bash
# MudBlazor Components
curl "https://r.jina.ai/https://mudblazor.com/docs/overview" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

**Bootstrap 5:**
```bash
# Bootstrap docs
curl "https://r.jina.ai/https://getbootstrap.com/docs/5.3/getting-started/introduction" \
  -H "Authorization: Bearer jina_60b9de152f954d5da898b7ebcf638bf2KAQ1_zudrsQqTCa9YRIltjMci9ZY"
```

## Technology Stack

**Standard stack for ALL projects:**
- **Runtime**: .NET 8+ (LTS)
- **Backend API**: ASP.NET Core Web API / Minimal APIs
- **Frontend Options**: 
  - Blazor Server or Blazor WebAssembly (preferred for SPA)
  - Razor Pages / MVC (for traditional web apps)
- **Database**: SQL Server (with Entity Framework Core)
- **ORM**: Entity Framework Core 8+
- **Authentication**: ASP.NET Core Identity / Duende IdentityServer
- **UI Components**: 
  - **Blazor**: MudBlazor (Material Design, excellent docs)
  - **Razor/MVC**: Bootstrap 5
- **Payments**: PayFast, Paystack, or Stitch (see Payment Integration section)
- **Hosting**: Azure App Service, Digital Ocean App Platform, or IIS
- **CI/CD**: GitHub Actions / Azure DevOps Pipelines
- **Testing**: xUnit, Playwright (E2E), Moq

## Payment Integration (South Africa)

### Payment Gateway Comparison

| Feature | PayFast | Paystack | Stitch |
|---------|---------|----------|--------|
| **Best For** | General SA businesses | Multi-country Africa | Enterprise/Open Banking |
| **API Type** | REST | REST | GraphQL |
| **.NET SDK** | ✅ Official | ✅ Community | ❌ Custom integration |
| **Cards** | ✅ | ✅ | ✅ |
| **Instant EFT** | ✅ | Bank Transfer | Pay by Bank APIs |
| **Digital Wallets** | ❌ | ❌ | ✅ Apple/Google/Samsung Pay |
| **Subscriptions** | ✅ | ✅ | ✅ DebiCheck |
| **Crypto** | ❌ | ❌ | ✅ |
| **Documentation** | Good | Excellent | Good |

### PayFast (Recommended for most projects)

PayFast is the most established payment gateway in South Africa with official .NET support.

**NuGet Packages:**
```bash
dotnet add package PayFast
dotnet add package PayFast.AspNetCore
```

**Features:**
- Credit/Debit cards (Visa, Mastercard, Amex)
- Instant EFT
- Subscriptions & Recurring Billing
- Debit Orders
- Mobicred

**Configuration:**
```csharp
// appsettings.json
{
  "PayFast": {
    "MerchantId": "",      // Set via environment variable
    "MerchantKey": "",     // Set via environment variable
    "PassPhrase": "",      // Set via environment variable
    "ProcessUrl": "https://www.payfast.co.za/eng/process",
    "ValidateUrl": "https://www.payfast.co.za/eng/query/validate",
    "ReturnUrl": "",
    "CancelUrl": "",
    "NotifyUrl": ""        // ITN callback URL
  }
}

// Program.cs
builder.Services.Configure<PayFastSettings>(
    builder.Configuration.GetSection("PayFast"));
```

**Sandbox**: Use `https://sandbox.payfast.co.za/eng/process` for testing

### Paystack (Multi-country Africa)

Paystack (owned by Stripe) supports Nigeria, Ghana, South Africa, and Kenya with excellent documentation.

**NuGet Package:**
```bash
dotnet add package Paystack.Net
```

**Features:**
- Credit/Debit cards
- Bank Transfer
- QR Payments (SA)
- Recurring payments/subscriptions
- 100% API coverage in .NET SDK

**Configuration:**
```csharp
// appsettings.json
{
  "Paystack": {
    "SecretKey": "",       // Set via environment variable
    "PublicKey": "",       // Set via environment variable
    "CallbackUrl": "",
    "WebhookSecret": ""
  }
}

// Usage
using PayStack.Net;

public class PaymentService
{
    private readonly PayStackApi _paystack;
    
    public PaymentService(IOptions<PaystackSettings> settings)
    {
        _paystack = new PayStackApi(settings.Value.SecretKey);
    }
    
    public async Task<string> InitializePayment(string email, int amountInCents)
    {
        var response = _paystack.Transactions.Initialize(email, amountInCents);
        if (response.Status)
            return response.Data.AuthorizationUrl;
        throw new Exception(response.Message);
    }
    
    public async Task<bool> VerifyPayment(string reference)
    {
        var response = _paystack.Transactions.Verify(reference);
        return response.Status && response.Data.Status == "success";
    }
}
```

**Supported Currencies:** NGN, GHS, ZAR, KES

### Stitch (Enterprise/Open Banking)

Stitch is ideal for enterprise businesses needing Pay-by-Bank, digital wallets, or open banking features.

**No official .NET SDK** - integrate via GraphQL API with HttpClient.

**Features:**
- Pay by Bank (Capitec Pay, Absa Pay, Nedbank Direct API)
- Digital Wallets (Apple Pay, Google Pay, Samsung Pay)
- Cards
- DebiCheck & Debit Orders
- Crypto payments
- LinkPay (one-click returning payments)

**Configuration:**
```csharp
// appsettings.json
{
  "Stitch": {
    "ClientId": "",        // Set via environment variable
    "ClientSecret": "",    // Set via environment variable
    "ApiUrl": "https://api.stitch.money/graphql"
  }
}

// Custom GraphQL client integration
public class StitchPaymentService
{
    private readonly HttpClient _httpClient;
    private readonly StitchSettings _settings;
    
    public StitchPaymentService(HttpClient httpClient, IOptions<StitchSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
    }
    
    public async Task<string> GetAccessTokenAsync()
    {
        // OAuth2 client credentials flow
        var tokenRequest = new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _settings.ClientId,
            ["client_secret"] = _settings.ClientSecret,
            ["scope"] = "client_paymentrequest"
        };
        
        var response = await _httpClient.PostAsync(
            "https://secure.stitch.money/connect/token",
            new FormUrlEncodedContent(tokenRequest));
            
        var content = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return content.AccessToken;
    }
    
    public async Task<PaymentInitiationResponse> InitiatePaymentAsync(
        string token, decimal amount, string reference)
    {
        var query = @"
            mutation CreatePaymentRequest($amount: MoneyInput!, $reference: String!) {
                clientPaymentInitiationRequestCreate(input: {
                    amount: $amount,
                    payerReference: $reference
                }) {
                    paymentInitiationRequest {
                        id
                        url
                    }
                }
            }";
            
        // Execute GraphQL mutation...
    }
}
```

**Documentation:** https://docs.stitch.money

### Choosing a Payment Gateway

- **PayFast**: Default choice for SA .NET projects - official SDK, well-established
- **Paystack**: Choose if expanding to Nigeria, Ghana, Kenya or need excellent docs
- **Stitch**: Choose for enterprise, Pay-by-Bank, or digital wallet requirements

## Critical Rules for ALL Agents

### NO HARDCODES, NO PLACEHOLDERS, NO FALLBACKS

**NEVER:**
- Hardcode API keys, prices, or configuration values
- Use placeholder text like "Coming soon" or "TODO"
- Skip features to "add later"
- Use fallback implementations
- Continue when blocked

**ALWAYS:**
- Use configuration/environment variables for ALL configuration
- Research with Context7/Jina before implementing
- Invoke stuck agent when blocked
- Complete features fully before moving on
- Test your work thoroughly

### Configuration & Environment Variables

ALL dynamic values MUST use configuration (appsettings.json + environment variables):

```csharp
// ✅ CORRECT - Using IConfiguration with Options Pattern
public class PaymentSettings
{
    public string MerchantId { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}

// In Program.cs
builder.Services.Configure<PaymentSettings>(
    builder.Configuration.GetSection("Payment"));

// Injected into services
public class PaymentService
{
    private readonly PaymentSettings _settings;
    
    public PaymentService(IOptions<PaymentSettings> settings)
    {
        _settings = settings.Value;
    }
}

// ❌ WRONG - NEVER DO THIS
var merchantId = "10000100"; // HARDCODED
var price = "R299/mo"; // HARDCODED
var appName = "My App"; // HARDCODED
```

**appsettings.json structure:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "PayFast": {
    "MerchantId": "",
    "MerchantKey": "",
    "PassPhrase": "",
    "ProcessUrl": "https://www.payfast.co.za/eng/process",
    "ReturnUrl": "",
    "CancelUrl": "",
    "NotifyUrl": ""
  },
  "Paystack": {
    "SecretKey": "",
    "PublicKey": "",
    "CallbackUrl": ""
  },
  "Stitch": {
    "ClientId": "",
    "ClientSecret": "",
    "ApiUrl": "https://api.stitch.money/graphql"
  },
  "AppSettings": {
    "AppName": "",
    "BaseUrl": ""
  }
}
```

### When to Invoke Stuck Agent

Invoke **stuck** agent IMMEDIATELY if:
- Need API key from user
- Build or deployment fails
- Unclear requirements
- Multiple valid approaches exist
- Any technical blocker
- Security issue found
- Test failures
- Cannot proceed autonomously

**DO NOT:**
- Try workarounds first
- Use placeholder implementations
- Skip the feature
- Make assumptions
- Continue without resolution

## OpenRouter Integration

ALL agents use OpenRouter for AI tasks.

**API Key**: Provided via environment variable
```
OPENROUTER_API_KEY=
```

**Usage**: Configure in each agent's implementation via IConfiguration

## Linear Integration

Software Architect creates Linear tickets for development tasks.

**Linear CLI Authentication**:
```bash
linear auth
```

**Ticket Creation**: Software Architect handles this

## GitHub CLI

DevOps Engineer uses GitHub CLI for repository and CI/CD management.

**Authentication**:
```bash
gh auth login
```

**Usage**: DevOps Engineer handles repository setup, secrets, and actions

## Azure CLI (if using Azure)

DevOps Engineer uses Azure CLI for deployment.

**Authentication**:
```bash
az login
```

**Usage**: DevOps Engineer handles App Service creation, configuration, and deployment

## Digital Ocean CLI (if using DO)

DevOps Engineer uses Digital Ocean CLI for deployment.

**Authentication**:
```bash
doctl auth init
```

**Usage**: DevOps Engineer handles app creation, env vars, and deployment

## .NET CLI Commands

Common commands for development:

```bash
# Create new solution
dotnet new sln -n MySolution

# Create projects
dotnet new webapi -n MyApp.Api
dotnet new blazorserver -n MyApp.Web
dotnet new blazorwasm -n MyApp.Client
dotnet new classlib -n MyApp.Core
dotnet new xunit -n MyApp.Tests

# Add projects to solution
dotnet sln add MyApp.Api/MyApp.Api.csproj

# Add packages
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package PayFast
dotnet add package PayFast.AspNetCore
dotnet add package Paystack.Net
dotnet add package MudBlazor

# Run application
dotnet run --project MyApp.Api

# Run with hot reload
dotnet watch --project MyApp.Web

# Run tests
dotnet test

# Publish for deployment
dotnet publish -c Release -o ./publish

# EF Core migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Playwright for E2E Testing

**QA Engineer** and **Backend Developer** use Playwright to:
- Test UI in browser
- Take screenshots for verification
- Verify application functionality
- Monitor application visually

**Installation:**
```bash
dotnet new nunit -n MyApp.E2ETests
dotnet add package Microsoft.Playwright.NUnit
playwright install
```

**Usage**: Via test projects with Playwright commands

## Communication Between Agents

### Document Flow
1. **CPO** → Creates `business-concept.md`
2. **Product Manager** → Creates `prd.md` (uses business-concept)
3. **Marketing** → Creates `brand-guidelines.md` (uses PRD)
4. **UX Designer** → Creates `style-guide.md` (uses brand-guidelines)
5. **Product Designer** → Creates `ui-designs.md` (uses style-guide + PRD)
6. **Software Architect** → Creates `technical-architecture.md` + Linear tickets (uses ui-designs + PRD)
7. **DBA** → Creates database schema & EF Core models (uses architecture)
8. **Frontend Dev** → Implements Blazor/Razor pages (uses ui-designs + style-guide)
9. **Backend Dev** → Implements API controllers & services (uses architecture + schema)
10. **Security Engineer** → Creates `security-report.md` (reviews code)
11. **QA Engineer** → Creates `test-report.md` (tests implementation)
12. **DevOps Engineer** → Creates `deployment-guide.md` (deploys to production)

### Handoff Points
Each agent's output becomes the next agent's input. **Read previous agent's deliverables before starting!**

## Quality Standards

### Code Quality
- C# nullable reference types enabled
- .NET Analyzers / StyleCop.Analyzers configured
- EditorConfig for consistent formatting
- No Debug.WriteLine or Console.WriteLine in production
- Proper exception handling with custom exceptions
- FluentValidation for input validation
- Use async/await properly throughout

### Project Structure
```
MySolution/
├── src/
│   ├── MyApp.Api/           # ASP.NET Core Web API
│   ├── MyApp.Web/           # Blazor Server / Razor Pages
│   ├── MyApp.Client/        # Blazor WebAssembly (if SPA)
│   ├── MyApp.Core/          # Domain models, interfaces
│   ├── MyApp.Infrastructure/ # EF Core, payment services
│   └── MyApp.Application/    # Business logic, services
├── tests/
│   ├── MyApp.UnitTests/
│   ├── MyApp.IntegrationTests/
│   └── MyApp.E2ETests/
├── MySolution.sln
└── Directory.Build.props
```

### Security
- No secrets in code or appsettings.json committed to git
- Use User Secrets for local development (`dotnet user-secrets`)
- Azure Key Vault / environment variables for production
- Input sanitisation and parameterised queries (EF Core handles this)
- ASP.NET Core Identity for authentication
- Authorisation policies on all protected endpoints
- HTTPS enforced
- Security headers via middleware (CSP, HSTS, etc.)
- Anti-forgery tokens for forms
- Payment webhook signature validation (ITN, Paystack webhooks, Stitch webhooks)

### Testing
- Unit tests with xUnit and Moq
- Integration tests with WebApplicationFactory
- E2E tests with Playwright
- All tests passing before deployment
- Coverage > 80% (where applicable)

### Performance
- Page load < 3s
- API response < 200ms (p95)
- Async all the way down
- Response caching where appropriate
- EF Core query optimisation (no N+1 queries)
- IMemoryCache / IDistributedCache for caching

### Accessibility
- WCAG AA compliance
- Keyboard navigation
- Screen reader support
- High contrast mode
- Touch targets 44px+
- ARIA labels on all interactive elements

## Success Metrics

A business is successfully completed when:
- ✅ All PRD requirements implemented
- ✅ All tests passing
- ✅ Security review passed
- ✅ NO hardcoded values
- ✅ ALL configuration via appsettings + env vars
- ✅ Application deployed to production
- ✅ Performance targets met
- ✅ Accessibility standards met
- ✅ Documentation complete
- ✅ Payment integration tested with sandbox/test mode

## Emergency Protocols

### If Application Won't Start
1. Check terminal/logs (`dotnet run` output, Event Viewer, Application Insights)
2. Verify environment variables and connection strings
3. Check dependencies installed (`dotnet restore`)
4. Verify database migrations applied
5. Invoke stuck agent with error details

### If Tests Fail
1. Screenshot the failure
2. Copy error logs and stack traces
3. Document reproduction steps
4. Invoke stuck agent

### If Security Issue Found
1. STOP immediately
2. Document the vulnerability
3. Assess severity
4. Invoke stuck agent URGENTLY

### If Deployment Fails
1. Check deployment logs (Azure Portal / DO dashboard)
2. Verify environment variables in hosting platform
3. Check build succeeded (`dotnet publish`)
4. Verify database connectivity from deployed environment
5. Invoke stuck agent with details

### If Payment Integration Fails
1. Check sandbox vs production URLs/keys
2. Verify webhook/callback endpoint is publicly accessible
3. Check merchant credentials and API keys
4. Review payment gateway dashboard for transaction logs
5. Validate webhook signatures
6. Invoke stuck agent with error codes/messages

---

**This shared memory ensures all agents work cohesively toward the same goal: building production-ready .NET applications for South African businesses!** 🚀
