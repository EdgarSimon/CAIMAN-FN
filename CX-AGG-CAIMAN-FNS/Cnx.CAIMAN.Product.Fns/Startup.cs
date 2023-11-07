﻿using Cnx.CAIMAN.Product.Fns.Core.Repositories;
using Cnx.CAIMAN.Product.Fns.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEO.Utilities.Helpers;
using NEO.Utilities.Helpers.HelpersRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Cnx.CAIMAN.Product.Fns.Startup))]
namespace Cnx.CAIMAN.Product.Fns
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
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
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
