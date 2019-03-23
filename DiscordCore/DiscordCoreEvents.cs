using Discord;
using Discord.WebSocket;
using DiscordCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCore
{
    public static class DiscordCoreEventsFactory
    {
        public static DiscordCoreEvents Create(DiscordSocketClient discordClient)
        {
            return new DiscordCoreEvents(discordClient);
        }
    }
}