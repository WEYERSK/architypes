# PayFast Configuration Setup Script
# Run this script to configure PayFast credentials using .NET User Secrets

Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "  Architypes - PayFast Configuration Setup" -ForegroundColor Cyan
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host ""

# Check if we're in the correct directory
if (-Not (Test-Path "src/Architypes.Web/Architypes.Web.csproj")) {
    Write-Host "Error: Please run this script from the solution root directory" -ForegroundColor Red
    exit 1
}

Write-Host "This script will configure PayFast credentials using .NET User Secrets." -ForegroundColor Yellow
Write-Host "User Secrets are stored securely on your local machine and never committed to git." -ForegroundColor Yellow
Write-Host ""

# Initialize user secrets
Write-Host "Initializing user secrets..." -ForegroundColor Green
Set-Location "src/Architypes.Web"
dotnet user-secrets init
Set-Location "../.."
Write-Host ""

# Prompt for PayFast credentials
Write-Host "Enter your PayFast credentials:" -ForegroundColor Cyan
Write-Host "(Press Enter to skip and use sandbox/empty values)" -ForegroundColor Gray
Write-Host ""

$merchantId = Read-Host "PayFast Merchant ID"
$merchantKey = Read-Host "PayFast Merchant Key"
$passphrase = Read-Host "PayFast Passphrase (optional)"

# Set user secrets
Set-Location "src/Architypes.Web"

if ($merchantId) {
    dotnet user-secrets set "PayFast:MerchantId" "$merchantId"
    Write-Host "✓ Merchant ID configured" -ForegroundColor Green
}

if ($merchantKey) {
    dotnet user-secrets set "PayFast:MerchantKey" "$merchantKey"
    Write-Host "✓ Merchant Key configured" -ForegroundColor Green
}

if ($passphrase) {
    dotnet user-secrets set "PayFast:Passphrase" "$passphrase"
    Write-Host "✓ Passphrase configured" -ForegroundColor Green
}

Set-Location "../.."

Write-Host ""
Write-Host "===============================================" -ForegroundColor Cyan
Write-Host "Configuration complete!" -ForegroundColor Green
Write-Host ""
Write-Host "To view your configured secrets, run:" -ForegroundColor Yellow
Write-Host "  cd src/Architypes.Web" -ForegroundColor Gray
Write-Host "  dotnet user-secrets list" -ForegroundColor Gray
Write-Host ""
Write-Host "To remove a secret, run:" -ForegroundColor Yellow
Write-Host "  dotnet user-secrets remove 'PayFast:MerchantId'" -ForegroundColor Gray
Write-Host ""
Write-Host "You can now run the application with:" -ForegroundColor Yellow
Write-Host "  dotnet run --project src/Architypes.Web" -ForegroundColor Gray
Write-Host "===============================================" -ForegroundColor Cyan
