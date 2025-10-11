namespace Omni.Modules
{
    [Group("temporary-voice-channels", "Temporary voice channels management.")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class TemporaryVoiceChannelsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly EmbedFactory _embedFactory;
        private static readonly HashSet<ulong> _hubs = [];
        private static readonly Dictionary<ulong, ulong> _temporaryChannels = [];

        public TemporaryVoiceChannelsModule(DiscordSocketClient discordSocketClient, EmbedFactory embedFactory)
        {
            _discordSocketClient = discordSocketClient;
            _embedFactory = embedFactory;
            _discordSocketClient.UserVoiceStateUpdated += HandleTemporaryVoiceChannelsAsync;
        }

        [SlashCommand("add-hub", "Add a hub.")]
        public async Task AddHubAsync(IVoiceChannel channel)
        {
            if (_hubs.Add(channel.Id))
                await RespondAsync(embed: _embedFactory.Success($"Hub {channel.Name} added."), ephemeral: true);
            else
                await RespondAsync(embed: _embedFactory.Error($"{channel.Name} is already a hub."), ephemeral: true);
        }

        [SlashCommand("remove-hub", "Remove a hub.")]
        public async Task RemoveHubAsync(IVoiceChannel channel)
        {
            if (_hubs.Remove(channel.Id))
                await RespondAsync(embed: _embedFactory.Success($"Hub {channel.Name} removed."), ephemeral: true);
            else
                await RespondAsync(embed: _embedFactory.Error($"{channel.Name} is not a hub."), ephemeral: true);
        }

        [SlashCommand("list-hubs", "List hubs.")]
        public async Task ListHubsAsync()
        {
            await RespondAsync(embed: _embedFactory.Hubs(_hubs), ephemeral: true);
        }

        private async Task HandleTemporaryVoiceChannelsAsync(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            if (user is not SocketGuildUser guildUser) return;
            if (after.VoiceChannel is not null && _hubs.Contains(after.VoiceChannel.Id) && !_temporaryChannels.ContainsKey(guildUser.Id))
            {
                var categoryIdentifier = after.VoiceChannel.CategoryId;
                var temporaryChannel = await guildUser.Guild.CreateVoiceChannelAsync($"{guildUser.Username}'s channel", properties =>
                {
                    if (categoryIdentifier is not null)
                        properties.CategoryId = categoryIdentifier;
                });
                _temporaryChannels[guildUser.Id] = temporaryChannel.Id;
                await guildUser.ModifyAsync(properties => properties.Channel = temporaryChannel);
            }
            if (before.VoiceChannel is not null && _temporaryChannels.ContainsValue(before.VoiceChannel.Id) && before.VoiceChannel.ConnectedUsers.Count == 0)
            {
                await before.VoiceChannel.DeleteAsync();
                var temporaryChannel = _temporaryChannels.FirstOrDefault(kvp => kvp.Value == before.VoiceChannel.Id);
                if (!temporaryChannel.Equals(default(KeyValuePair<ulong, ulong>)))
                    _temporaryChannels.Remove(temporaryChannel.Key);
            }
        }
    }
}
