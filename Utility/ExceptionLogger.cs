using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class ExceptionLogger
    {
        private static bool _isConfigured = false;
        private static IConfiguration _configuration;
        public static void LogException(Exception ex, string context = "")
        {
            InsertRole(context, ex.Message);
            if (!_isConfigured)
            {
                ConfigureSerilog();
                _isConfigured = true;
            }
            if (ex != null)
            {
                Log.Error("Error in {Context}: {Message}", context, ex.Message);
            }
        }

        public static void ConfigureSerilog()
        {
            var logDir = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);  // Safe, works on all environments
            }

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build())
                .CreateLogger();
        }

        public static void InsertRole(string context, string ExceptionMessage)
        {
            try
            {
                string connStr = ApplicationSettings.DefaultConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("usp_InsertErrorLog", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ErrorPath", context);
                        cmd.Parameters.AddWithValue("@ErrorDiscription", ExceptionMessage);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }


}
