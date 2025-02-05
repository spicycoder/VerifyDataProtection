using AspNetCore.Swagger.Themes;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.X509Certificates;
using VerifyDataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

string certThumbprint = "YOUR_CERTIFICATE_THUMBPRINT";
X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
store.Open(OpenFlags.ReadOnly);
X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, false);
store.Close();

//var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Certificate:Path"]);
//var certificatePassword = builder.Configuration["Certificate:Password"];
//var certificate = new X509Certificate2(certificatePath, certificatePassword);
var certificate = certCollection[0];

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
logger.LogInformation($"Certificate Thumbprint: {certificate.Thumbprint}");

var keysDirectory = Path.Combine(Directory.GetCurrentDirectory(), "DataProtection-Keys");
if (!Directory.Exists(keysDirectory))
{
    Directory.CreateDirectory(keysDirectory);
}

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysDirectory))
    .ProtectKeysWithCertificate(certificate)
    .SetApplicationName("VerifyDataProtection");

builder.Services.AddHostedService<KeyInspector>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Futuristic);
}

app.UseAuthorization();
app.MapControllers();
app.Run();