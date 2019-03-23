using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using DiscordCore.Configuration;

namespace DiscordCore
{
    public class PluginManager
    {
        private string xmlFilePath = "";
        private string apiToken = "";
        private List<PluginConfig> pluginConfigList = new List<PluginConfig>();
        private List<Plugin> pluginList = new List<Plugin>();

        public PluginManager()
        {
            xmlFilePath = DiscordCoreConfiguration.PluginConfigFilePath;
            apiToken = DiscordCoreConfiguration.DiscordApiToken;
        }

        public void Start()
        {
            DiscordSocketClient _client = new DiscordSocketClient();
            // Register the DLL resolver
            AppDomain.CurrentDomain.AssemblyResolve += PluginHelper.ResolveAssembly;
            _client.LoginAsync(TokenType.Bot, apiToken).Wait();
            _client.StartAsync().Wait();
            DiscordCoreEvents events = DiscordCoreEventsFactory.Create(_client);
            Reload();
            pluginList = PluginHelper.LoadPlugins(pluginConfigList);
            PluginHelper.InitializePlugins(pluginList, events);
            PluginHelper.StartPlugins(pluginList);
        }

        public void Reload()
        {
            pluginConfigList = PluginHelper.ParseConfigurationFile(xmlFilePath);
        }

        public void Stop()
        {
            PluginHelper.StopPlugins(pluginList);
        }
    }
}