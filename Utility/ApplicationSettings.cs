using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ApplicationSettings
    {
        private static IConfiguration _configuration;
        static ApplicationSettings() => _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory()) // Set the base path to the current directory
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Add appsettings.json
               .Build();


        public static string DefaultConnectionString
        {
            get { return _configuration.GetConnectionString("Databaseconnection"); }
        }
    }
}
