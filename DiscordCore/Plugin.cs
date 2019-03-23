using System.Reflection;
using DiscordCore.Interfaces;
using System;

namespace DiscordCore
{
    public class Plugin
    {
        public PluginConfig pluginConfig { get; set; }
        public Assembly Assembly { get; set; }
        public IDiscordCorePlugin Instance { get; set; }
        public PluginStatus Status { get; set; }
        public PluginLoadStatus LoadStatus { get; set; }
    }
}