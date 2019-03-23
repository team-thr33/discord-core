using System;
using DiscordCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DiscordCore.Tests
{
    [TestClass]
    public class ConfigurationFileTests
    {
        [TestMethod]
        public void ParseConfigFile()
        {
            List<PluginConfig> pluginConfigList;
            string xmlFilePath = "TestResources//PluginInfo.xml";
            pluginConfigList = DiscordCore.PluginHelper.ParseConfigurationFile(xmlFilePath);
            Assert.IsNotNull(pluginConfigList);
        }

        [TestMethod]
        public void UnableToFileConfig()
        {
            List<PluginConfig> pluginConfigList;
            string xmlFilePath = "TestResources//IncorrectPath.xml";
            pluginConfigList = PluginHelper.ParseConfigurationFile(xmlFilePath);
            Assert.IsTrue(pluginConfigList.Count == 0);
        }

        [TestMethod]
        public void InvalidConfigFile()
        {
            List<PluginConfig> pluginConfigList;
            string xmlFilePath = "TestResources//InvalidConfigFile.xml";
            pluginConfigList = PluginHelper.ParseConfigurationFile(xmlFilePath);
            Assert.IsTrue(pluginConfigList.Count == 0);
        }
    }
}