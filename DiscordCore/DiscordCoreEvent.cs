using System;
using DiscordCore.Interfaces;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DiscordCore
{
    public class DiscordCoreEvents : IDiscordCoreEvents
    {
        private DiscordSocketClient _discordClient;
        private SocketGuild _guild;

        public DiscordCoreEvents(DiscordSocketClient discordClient)
        {
            _discordClient = discordClient;
            SetupEvents();
        }

        public event EventHandler<NewMessageEventArgs> NewMessage;

        public event EventHandler<DeletedMessageEventArgs> DeletedMessage;

        public event EventHandler<UserStateChangedEventArgs> UserStateChanged;

        public event EventHandler<NewReactionEventArgs> NewReaction;

        public event EventHandler<NewReactionClearedEventArgs> ClearedReaction;

        public event EventHandler<NewReactionDeletedtArgs> DeletedRection;

        public event EventHandler<UserNameChangedEventArgs> UserNameChanged;

        private void SetupEvents()
        {
            _discordClient.GuildMembersDownloaded += async (guild) =>
            {
                await guild.DownloadUsersAsync();
                _guild = guild;
                _discordClient.GuildMemberUpdated += UserUpdated;
                _discordClient.MessageReceived += MessageReceived;
                _discordClient.MessageDeleted += MessageDeleted;
                _discordClient.ReactionAdded += ReactionAdded;
                _discordClient.ReactionRemoved += ReactionRemoved;
                _discordClient.ReactionsCleared += ReactionsCleared;
            };
        }

        private Task ReactionsCleared(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            OnRaiseCustomEvent(new NewReactionClearedEventArgs()
            {
                ChannelId = arg2.Id,
                MessageId = arg1.Id
            });

            return Task.CompletedTask;
        }

        private Task ReactionRemoved(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            OnRaiseCustomEvent(new NewReactionDeletedtArgs()
            {
                UserId = arg3.UserId,
                ChannelId = arg2.Id,
                MessageId = arg1.Id,
                Emote = arg3.Emote.Name
            });
            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            Console.WriteLine(GetMessage(arg2.Id, arg1.Id));
            OnRaiseCustomEvent(new NewReactionEventArgs()
            {
                UserId = arg3.UserId,
                ChannelId = arg2.Id,
                MessageId = arg1.Id,
                Emote = arg3.Emote.Name
            });
            return Task.CompletedTask;
        }

        private Task UserUpdated(SocketUser beforeUser, SocketUser currentUser)
        {
            if (beforeUser.Status != currentUser.Status)
            {
                OnRaiseCustomEvent(new UserStateChangedEventArgs()
                {
                    UserId = beforeUser.Id,
                    OldState = beforeUser.Status.ToUserState(),
                    CurrentState = currentUser.Status.ToUserState()
                });
            }
            if (beforeUser.Username != currentUser.Username)
            {
                OnRaiseCustomEvent(new UserNameChangedEventArgs()
                {
                    UserId = beforeUser.Id,
                    OldName = beforeUser.Username,
                    CurrentName = currentUser.Username
                });
            }
            return Task.CompletedTask;
        }

        private Task MessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            OnRaiseCustomEvent(new DeletedMessageEventArgs()
            {
                UserId = 0,
                ChannelId = arg2.Id,
                DeletedMessage = arg1.HasValue ? arg1.Value.ToString() : string.Empty
            });

            return Task.CompletedTask;
        }

        private Task MessageReceived(SocketMessage arg)
        {
            OnRaiseCustomEvent(new NewMessageEventArgs()
            {
                UserId = arg.Author.Id,
                ChannelId = arg.Channel.Id,
                NewMessage = arg.ToString()
            });

            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        public void PostMessage(ulong channelId, string pluginName, string messageString)
        {
            string outputMessage = "**" + pluginName + "**" + Environment.NewLine;

            SocketTextChannel channel = _guild.GetTextChannel(channelId);
            channel.SendMessageAsync(outputMessage + messageString).Wait();
        }

        public void PostEmbeddedImage(ulong channelId, string pluginName, string imageURL)
        {
            string outputMessage = "**" + pluginName + "**" + Environment.NewLine;
            SocketTextChannel channel = _guild.GetTextChannel(channelId);

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithImageUrl(imageURL);

            var embed = embedBuilder.Build();

            try
            {
                channel.SendMessageAsync(string.Empty, false, embed).Wait();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
#if DEBUG
                channel.SendMessageAsync(message).Wait();
#endif
                Helpers.LogVerbose(message);
            }
        }

        public void PostEmbeddedImage(ulong channelId, string pluginName, string imageURL, string title)
        {
            string outputMessage = "**" + pluginName + "**" + Environment.NewLine;
            SocketTextChannel channel = _guild.GetTextChannel(channelId);

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle(title).WithImageUrl(imageURL);

            var embed = embedBuilder.Build();

            try
            {
                channel.SendMessageAsync(string.Empty, false, embed).Wait();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
#if DEBUG
                channel.SendMessageAsync(message).Wait();
#endif
                Helpers.LogVerbose(message);
            }
        }

        public void PostEmbedMessage(ulong channelId, string pluginName, string thumbNailURL, string mediaURL)
        {
            string outputMessage = "**" + pluginName + "**" + Environment.NewLine;

            SocketTextChannel channel = _guild.GetTextChannel(channelId);

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.ThumbnailUrl = thumbNailURL;
            embedBuilder.Url = mediaURL;

            var embed = embedBuilder.Build();

            try
            {
                channel.SendMessageAsync(string.Empty, false, embed).Wait();
            }
            catch (Exception ex)
            {
                string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
#if DEBUG
                channel.SendMessageAsync(message).Wait();
#endif
                Helpers.LogVerbose(message);
            }
        }

        public DiscordChannel GetTextChannel(string name)
        {
            if (_guild != null)
            {
                foreach (SocketGuildChannel channel in _guild.Channels)
                {
                    if (name == channel.Name)
                        return GetTextChannel(channel.Id);
                }
            }

            throw new NotYetInitializedException();
        }

        public string GetMessage(ulong channelId, ulong messageId)
        {
            var channel = _guild.GetTextChannel(channelId);
            var message = channel.GetMessageAsync(messageId);
            return message.Result.Content;
        }

        public DiscordChannel GetTextChannel(ulong id)
        {
            SocketTextChannel channel = _guild.GetTextChannel(id);
            return new DiscordChannel
            {
                Id = channel.Id,
                Name = channel.Name
            };
        }

        public List<DiscordUser> GetUsers()
        {
            List<DiscordUser> newUserList = new List<DiscordUser>();
            var userList = _guild.Users;
            foreach (var user in userList)
            {
                newUserList.Add(new DiscordUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Discriminator = user.Discriminator
                });
            }
            return newUserList;
        }

        public DiscordUser GetUser(ulong id)
        {
            SocketUser user = _discordClient.GetUser(id);
            DiscordUser discordUser = new DiscordUser
            {
                Id = id,
                Username = user.Username,
                Discriminator = user.Discriminator
            };

            return discordUser;
        }

        protected virtual void OnRaiseCustomEvent(NewMessageEventArgs e)
        {
            NewMessage?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(NewReactionEventArgs e)
        {
            NewReaction?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(NewReactionClearedEventArgs e)
        {
            ClearedReaction?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(NewReactionDeletedtArgs e)
        {
            DeletedRection?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(DeletedMessageEventArgs e)
        {
            DeletedMessage?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(UserStateChangedEventArgs e)
        {
            UserStateChanged?.Invoke(this, e);
        }

        protected virtual void OnRaiseCustomEvent(UserNameChangedEventArgs e)
        {
            UserNameChanged?.Invoke(this, e);
        }
    }
}