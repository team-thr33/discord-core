using DiscordCore.Interfaces;
using System;

namespace DiscordCore.Interfaces
{
    public interface IDiscordCorePlugin
    {
        string GetPluginName();

        string GetPluginDescription();

        void Start();

        void Initalize(IDiscordCoreEvents discordEventClient);

        void Stop();
    }
}