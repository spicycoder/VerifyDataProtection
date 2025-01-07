using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace DataProtectionKeyGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputDir = $"{AppContext.BaseDirectory}/DataProtection-Keys";
            string subjectName = "VerifyDataProtection";

            try
            {
                // Ensure the output directory exists
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                var certificates = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, false);
                var certificate = certificates
                    .OfType<X509Certificate2>()
                    .OrderByDescending(x => x.NotAfter)
                    .FirstOrDefault();

                if (certificate == null)
                {
                    throw new Exception("Certificate not found");
                }

                // Configure Data Protection
                var services = new ServiceCollection();
                var dpBuilder = services
                    .AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(outputDir))
                    .ProtectKeysWithCertificate(certificate)
                    .SetApplicationName(subjectName);

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