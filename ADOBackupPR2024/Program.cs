// See https://aka.ms/new-console-template for more information
using ADOBackupPR2024;
using CoreOPerations.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build;

var services = new ServiceCollection();
services.ConfigureADOServices(configuration.Invoke());
using var serviceProvide = services.BuildServiceProvider();

var adoService = serviceProvide.GetService<IADOOperations>();
var config = configuration.Invoke().GetSection("RestoreConfigs");


if (config.GetValue<bool>("IsLocalBackup"))
{
    await adoService.ADOCreatePullRequestBackup();
}
else
{
    await adoService.RestorePrsByDateTime();
}


Console.WriteLine();
