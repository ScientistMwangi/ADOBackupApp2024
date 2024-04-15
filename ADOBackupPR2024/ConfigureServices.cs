using ADODomainModels.models;
using CoreOPerations.interfaces;
using CoreOPerations.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;
using System.Globalization;

namespace ADOBackupPR2024
{
    public static class ConfigureServices
    {
        public static void ConfigureADOServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var config = configuration.GetSection("RestoreConfigs");
            var initialModel = new GitHttpClientModel
            {
                OrganizationUrl = config.GetValue<string>("OrganizationUrl"),
                PersonalAccessToken = config.GetValue<string>("PAT"),
                ProjectName = config.GetValue<string>("ProjectName"),
                RepositoryName = config.GetValue<string>("Repository"),
                StartDate = DateTime.Parse(config.GetValue<string>("StateFile"))//.ToUniversalTime()
            };

            services.AddSingleton<IADOOperations, ADOOperations>
                (
                x => new ADOOperations(initialModel)

                );
        }
    }
}

