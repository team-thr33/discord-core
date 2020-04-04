using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCore.Interfaces
{
    public enum UserState
    {
        Offline,
        Online,
        Idle,
        AFK,
        DoNotDisturb,
        Invisible
    }

    public class NotYetInitializedException : Exception
    {
    }

    public struct DiscordChannel
    {
        public ulong Id;
        public string Name;
    }

    public struct DiscordUser
    {
        public ulong Id;
        public string Username;
        public string Discriminator;
    }

    public class NewMessageEventArgs : EventArgs
    {
        public ulong UserId;
        public ulong ChannelId;
        public string NewMessage;
    }

    public class NewReactionEventArgs : EventArgs
    {
        public ulong UserId;
        public ulong ChannelId;
        public ulong MessageId;
        public string Emote;
    }

    public class NewReactionClearedEventArgs : EventArgs
    {
        public ulong ChannelId;
        public ulong MessageId;
    }

    public class NewReactionDeletedtArgs : EventArgs
    {
        public ulong UserId;
        public ulong ChannelId;
        public ulong MessageId;
        public string Emote;
    }

    public class DeletedMessageEventArgs : EventArgs
    {
        public ulong UserId;
        public ulong ChannelId;
        public string DeletedMessage;
    }

    public class UserStateChangedEventArgs : EventArgs
    {
        public ulong UserId;
        public UserState OldState;
        public UserState CurrentState;
    }

    public class UserNameChangedEventArgs : EventArgs
    {
        public ulong UserId;
        public string OldName;
        public string CurrentName;
    }
}