using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordCore.Interfaces
{
    public interface IDiscordCoreEvents
    {
        event EventHandler<NewMessageEventArgs> NewMessage;

        event EventHandler<DeletedMessageEventArgs> DeletedMessage;

        event EventHandler<UserStateChangedEventArgs> UserStateChanged;

        event EventHandler<UserNameChangedEventArgs> UserNameChanged;

        event EventHandler<NewReactionEventArgs> NewReaction;

        event EventHandler<NewReactionClearedEventArgs> ClearedReaction;

        event EventHandler<NewReactionDeletedtArgs> DeletedRection;

        void PostMessage(ulong channelId, string pluginName, string message);

        void PostEmbeddedImage(ulong channelId, string pluginName, string imageURL);

        void PostEmbeddedImage(ulong channelId, string pluginName, string imageURL, string title);

        void PostEmbedMessage(ulong channelId, string pluginName, string thumbNailURL, string mediaURL);

        DiscordUser GetUser(ulong id);

        List<DiscordUser> GetUsers();

        DiscordChannel GetTextChannel(ulong id);

        DiscordChannel GetTextChannel(string name);

        string GetMessage(ulong channelId, ulong messageId);
    }
}