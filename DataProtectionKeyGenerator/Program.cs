using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace DataProtectionKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Hardcoded values
            string outputDir = $"{AppContext.BaseDirectory}/DataProtection-Keys";
            string appName = "VerifyDataProtection";
            string certPath = "./Certificates/VerifyDataProtection.pfx";
            string certificatePassword = "YourPassword123!";

            try
            {
                // Ensure the output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the certificate with the provided password
                var certificate = new X509Certificate2(certPath, certificatePassword, X509KeyStorageFlags.Exportable);

                // Configure Data Protection
                var services = new ServiceCollection();
                var dpBuilder = services
                    .AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(outputDir))
                    .ProtectKeysWithCertificate(certificate)
                    .SetApplicationName(appName);

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var dataProtection = serviceProvider.GetService<IDataProtectionProvider>();
                    var protector = dataProtection.CreateProtector("Test");
                    var testData = protector.Protect("Test");
                    Console.WriteLine("Key generation and protection test successful");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}