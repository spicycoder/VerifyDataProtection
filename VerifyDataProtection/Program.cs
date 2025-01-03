using AspNetCore.Swagger.Themes;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.X509Certificates;
using VerifyDataProtection;

var builder = WebApplication.CreateBuilder(args);

var certificatePath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["Certificate:Path"]);
var certificatePassword = builder.Configuration["Certificate:Password"];
var certificate = new X509Certificate2(certificatePath, certificatePassword);

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(ModernStyle.Futuristic);
}

app.UseAuthorization();
app.MapControllers();
app.Run();