using Microsoft.Extensions.Configuration;
using NEO.Utilities.Helpers.HelpersRepositories;
using NEO.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Cnx.CAIMAN.Acknowledge.Fns.Repositories;
using Cnx.CAIMAN.Acknowledge.Fns.Core.Repositories;

[assembly: FunctionsStartup(typeof(Cnx.CAIMAN.Acknowledge.Fns.Startup))]
namespace Cnx.CAIMAN.Acknowledge.Fns
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(_configuration);
            builder.Services.AddSingleton<ISqlDapperHelper>(x =>
            {
                var ConnectionStringName = "CAIMAN_ConnectionString";
                return new SqlDapperHelper(ConnectionStringName, _configuration);
            });
            builder.Services.AddTransient<ILogInterfaceHelper, LogInterfaceHelper>();
            builder.Services.AddTransient<ISendToSapHelper, SendToSapHelper>();
            builder.Services.AddTransient<IResultadoRepository, ResultadoRepository>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();
            _configuration = config;
        }
    }
}
