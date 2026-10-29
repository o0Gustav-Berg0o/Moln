using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Läs valvets namn från konfigurationen (Environment variables i Azure).
// Lokalt finns inget värde, då hoppar vi över Key Vault och appen startar ändå.
string? keyVaultName = builder.Configuration["KeyVaultName"];

if (string.IsNullOrWhiteSpace(keyVaultName))
{
    Program.KeyVaultStatus = "Not Configured";
}
else if (keyVaultName.Contains("://") || keyVaultName.Contains('.'))
{
    // Bara namnet ska anges, t.ex. "hemligtvalvimolnet", inte hela URL:en.
    Program.KeyVaultStatus = "Invalid KeyVaultName";
}
else
{
    try
    {
        Uri vaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        DefaultAzureCredential credential = new DefaultAzureCredential();

        // Testa uppkopplingen först så att vi kan ge ett tydligt felmeddelande.
        SecretClient client = new SecretClient(vaultUri, credential);
        _ = client.GetPropertiesOfSecrets().Take(1).ToList();

        // Lägg till Key Vault som konfigurationskälla.
        // Den läggs sist och skriver därför över appsettings och Environment variables
        // för nycklar med samma namn.
        builder.Configuration.AddAzureKeyVault(vaultUri, credential);

        Program.KeyVaultStatus = "Connected Successfully";
    }
    catch (CredentialUnavailableException ex)
    {
        Program.KeyVaultStatus = "Authentication Failed (no credential available)";
        Program.KeyVaultError = ex.Message;
    }
    catch (AuthenticationFailedException ex)
    {
        Program.KeyVaultStatus = "Authentication Failed";
        Program.KeyVaultError = ex.Message;
    }
    catch (RequestFailedException ex) when (ex.Status is 401 or 403)
    {
        Program.KeyVaultStatus = "Access Denied";
        Program.KeyVaultError = ex.Message;
    }
    catch (Exception ex)
    {
        Program.KeyVaultStatus = "Error";
        Program.KeyVaultError = ex.Message;
    }
}

builder.Services.AddControllers();
builder.Services.AddSingleton<ConfigurationDemo.Services.IKeyVaultService, ConfigurationDemo.Services.KeyVaultService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

// Enkel statussida som används i övningen
app.MapGet("/keyvault-status", () => Results.Ok(new
{
    Status = Program.KeyVaultStatus,
    Error = Program.KeyVaultError,
    ConfiguredName = app.Configuration["KeyVaultName"]
}));

app.Run();

// Gör att controllers kan läsa statusen via Program.KeyVaultStatus
public partial class Program
{
    public static string KeyVaultStatus { get; set; } = "Not Configured";
    public static string? KeyVaultError { get; set; }
}
