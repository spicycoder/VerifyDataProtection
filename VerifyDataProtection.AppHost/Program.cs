var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.VerifyDataProtection>("verifydataprotection");

builder.Build().Run();
