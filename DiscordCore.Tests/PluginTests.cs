using System;
using DiscordCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;

namespace DiscordCore.Tests
{
    [TestClass]
    public class ConfigurationFile
    {
        //[TestMethod]
        public void LoadInvalidPlugin()
        {
            List<PluginConfig> pluginConfigList;
            List<Plugin> pluginList;
            pluginConfigList = new List<PluginConfig>()
            {
                new PluginConfig()
                {
                    DLLFile = "TestResources//PluginWithInvalidInterface.dll",
                    Enabled = true,
                    Name = "Example Plugin"
                }
            };

            pluginList = PluginHelper.LoadPlugins(pluginConfigList);
            Assert.IsTrue(pluginList.Count != 0);
        }

        [TestMethod]
        public void LoadValidPlugin()
        {
            List<PluginConfig> pluginConfigList;
            List<Plugin> pluginList;
            pluginConfigList = new List<PluginConfig>()
            {
                new PluginConfig()
                {
                    DLLFile = "TestResources//DiscordCore.Plugin.Example.dll",
                    Enabled = true,
                    Name = "Example Plugin"
                }
            };

            pluginList = PluginHelper.LoadPlugins(pluginConfigList);
            foreach (var plugin in pluginList)
            {
                PluginHelper.GetPluginType(plugin);
            }
        }
    }
}