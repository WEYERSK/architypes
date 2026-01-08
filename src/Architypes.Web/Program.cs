using Architypes.Application.Interfaces;
using Architypes.Application.Services;
using Architypes.Core.Configuration;
using Architypes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add MudBlazor services
builder.Services.AddMudServices();

// Configure settings
builder.Services.Configure<PayFastSettings>(
    builder.Configuration.GetSection(PayFastSettings.SectionName));
builder.Services.Configure<PricingSettings>(
    builder.Configuration.GetSection(PricingSettings.SectionName));

// Add DbContext
builder.Services.AddDbContext<ArchitypesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Architypes.Infrastructure")));

// Add session support for anonymous user tracking
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register services
builder.Services.AddScoped<IAssessmentService, AssessmentService>();
builder.Services.AddScoped<IPaymentService, PayFastPaymentService>();
builder.Services.AddScoped<IPdfService, PdfService>();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedDataAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
