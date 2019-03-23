using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCore.Configuration
{
    public static class DiscordCoreConfiguration
    {
        public static string DiscordApiToken
        {
            get
            {
                return ConfigurationManager.AppSettings["DiscordApiToken"].ToString();
            }
        }

        public static string PluginConfigFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["PluginConfigFilePath"].ToString();
            }
        }
    }
}