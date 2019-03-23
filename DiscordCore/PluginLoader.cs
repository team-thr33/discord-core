using DiscordCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using DiscordCore.Configuration;
using System.Linq;

namespace DiscordCore
{
    public static class PluginHelper
    {
        private static List<string> assemblySearchDirectories = new List<string>();

        public static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Split(new[] { ',' })[0] + ".dll";
            foreach(string directory in assemblySearchDirectories)
            {
                string absolutePath = Path.Combine(directory, dllName);
                if (File.Exists(absolutePath))
                    return Assembly.LoadFrom(absolutePath);
            }
            return null;
        }

        private static void LoadDll(Plugin plugin)
        {
            // Load the DLL
            string path = Path.Combine(Directory.GetCurrentDirectory(), Path.GetDirectoryName(DiscordCoreConfiguration.PluginConfigFilePath), plugin.pluginConfig.DLLFile);
            plugin.Assembly = Assembly.LoadFile(path);
            
            // Add the DLL directory to the search directories
            assemblySearchDirectories.Add(Path.GetDirectoryName(path));
        }

        private static void InstanciatePlugin(Plugin plugin)
        {
            var types = plugin.Assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.GetInterface(nameof(IDiscordCorePlugin)) != null)
                {
                    plugin.Instance = (IDiscordCorePlugin)Activator.CreateInstance(type);
                }
            }
        }

        public static Type GetPluginType(Plugin plugin)
        {
            Type pluginType = null;
            try
            {
                var types = plugin.Assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.GetInterface(nameof(IDiscordCorePlugin)) == typeof(IDiscordCorePlugin))
                    {
                        pluginType = type;
                        plugin.Status = PluginStatus.VALIDATED;
                    }
                    else
                    {
                        plugin.Status = PluginStatus.INVALID;
                    }
                }
            }
            catch (Exception)
            {
                plugin.Status = PluginStatus.INVALID;
            }

            return pluginType;
        }

        public static void InitializePlugins(List<Plugin> pluginList, DiscordCoreEvents discordCoreEvents)
        {
            foreach (var plugin in pluginList)
            {
                plugin.Instance.Initalize(discordCoreEvents);
            }
        }

        public static List<PluginConfig> ParseConfigurationFile(string xmlConfigFilePath)
        {
            string configXmlString = "";
            List<PluginConfig> pluginConfigList = new List<PluginConfig>();
            try
            {
                configXmlString = File.ReadAllText(xmlConfigFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                Helpers.LogDebug($"Unable to locate config directory [{xmlConfigFilePath}]");
            }
            catch (FileNotFoundException)
            {
                Helpers.LogDebug("Unable to find dll");
            }

            if (!string.IsNullOrEmpty(configXmlString))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<PluginConfig>), new XmlRootAttribute("PluginConfiguration"));
                StringReader stringReader = new StringReader(configXmlString);
                try
                {
                    pluginConfigList = (List<PluginConfig>)serializer.Deserialize(stringReader);
                }
                catch (InvalidOperationException)
                {
                    Helpers.LogDebug("Incorrectly Formed XML File");
                }
            }
            return (pluginConfigList);
        }

        public static List<Plugin> LoadPlugins(List<PluginConfig> pluginConfigList)
        {
            List<Plugin> pluginList = new List<Plugin>();
            foreach (var pluginConfig in pluginConfigList)
            {
                if (pluginConfig.Enabled)
                {
                    Plugin plugin = new Plugin() { pluginConfig = pluginConfig };
                    LoadDll(plugin);
                    InstanciatePlugin(plugin);

                    if (plugin.Instance != null)
                    {
                        plugin.Status = PluginStatus.LOADED;
                        Helpers.LogDebug("Sucessfully Loaded: " + plugin.Instance.GetPluginName());
                    }
                    else if (plugin.Instance == null)
                    {
                        plugin.Status = PluginStatus.ERROR;
                        Helpers.LogDebug("Unable to Instantiate Plugin" + plugin.pluginConfig.Name);
                    }
                    else
                    {
                        plugin.Status = PluginStatus.ERROR;
                        Helpers.LogDebug("Unable to load DLL " + plugin.pluginConfig.Name);
                    }

                    pluginList.Add(plugin);
                }
            }
            return pluginList;
        }

        public static void StartPlugins(List<Plugin> pluginList)
        {
            foreach (var plugin in pluginList)
            {
                Helpers.LogDebug("Starting: " + plugin.Instance.GetPluginName());
                plugin.Instance.Start();
                plugin.Status = PluginStatus.RUNNING;
                Helpers.LogDebug("Sucessfully Started: " + plugin.Instance.GetPluginName());
            }
        }

        public static void StopPlugins(List<Plugin> pluginList)
        {
            foreach (var plugin in pluginList)
            {
                plugin.Instance.Stop();
                plugin.Status = PluginStatus.STOPPED;
                Helpers.LogDebug("Sucessfully Stopped: " + plugin.Instance.GetPluginName());
            }
        }
    }
}