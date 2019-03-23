using DiscordCore.Interfaces;
using System;

namespace DiscordCore.Plugin.Example
{
    internal class ExamplePlugin : IDiscordCorePlugin
    {
        private string _name = "ExamplePlugin";

        private string _description = "This is an Example Plugin for the Discord Plugin Framework";

        private DiscordChannel _botChannel;

        private IDiscordCoreEvents _discordEventClient;

        public void Initalize(IDiscordCoreEvents discordEventClient)
        {
            _discordEventClient = discordEventClient;
            _discordEventClient.NewMessage += _discordEventClient_newMessage;
            _discordEventClient.UserStateChanged += _discordEventClient_UserStateChanged;
            _discordEventClient.ClearedReaction += _discordEventClient_ClearedReaction;
            _discordEventClient.DeletedMessage += _discordEventClient_DeletedMessage;
            _discordEventClient.NewReaction += _discordEventClient_NewReaction;
        }

        private void _discordEventClient_NewReaction(object sender, NewReactionEventArgs e)
        {
            // var message = _discordEventClient.
        }

        private void _discordEventClient_DeletedMessage(object sender, DeletedMessageEventArgs e)
        {
        }

        private void _discordEventClient_ClearedReaction(object sender, NewReactionClearedEventArgs e)
        {
        }

        private void _discordEventClient_UserStateChanged(object sender, UserStateChangedEventArgs e)
        {
            GetBotChannel();
            DiscordUser user = _discordEventClient.GetUser(e.UserId);
            _discordEventClient.PostMessage(_botChannel.Id, _name, $"{user.Username} is now {e.CurrentState.ToString()}");
        }

        private void _discordEventClient_newMessage(object sender, NewMessageEventArgs e)
        {
            if (e.NewMessage.Equals("@Example", StringComparison.InvariantCultureIgnoreCase))
            {
                DiscordUser user = _discordEventClient.GetUser(e.UserId);
                _discordEventClient.PostMessage(e.ChannelId, _name, $"New Message from Example Plugin, triggered from {user.Username}, {user.Discriminator}");
            }
        }

        private void GetBotChannel()
        {
            _botChannel = _discordEventClient.GetTextChannel("plugintesting");
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public string GetPluginName()
        {
            return _name;
        }

        public string GetPluginDescription()
        {
            return _description;
        }
    }
}