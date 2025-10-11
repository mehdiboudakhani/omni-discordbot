namespace Omni.Modules
{
    [Group("temporary-voice-channels", "Temporary voice channels management.")]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class TemporaryVoiceChannelsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly DiscordSocketClient _discordSocketClient;
        private static readonly HashSet<ulong> _hubs = [];
        private static readonly Dictionary<ulong, ulong> _temporaryChannels = [];

        public TemporaryVoiceChannelsModule(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _discordSocketClient.UserVoiceStateUpdated += HandleTemporaryVoiceChannelsAsync;
        }

        [SlashCommand("add-hub", "Add a hub.")]
        public async Task AddHubAsync(IVoiceChannel channel)
        {
            if (_hubs.Add(channel.Id))
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Temporary voice channels")
                    .WithDescription($"Hub {channel.Name} added.")
                    .WithColor(Color.Green)
                    .Build();
                await RespondAsync(embed: embed, ephemeral: true);
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Temporary voice channels")
                    .WithDescription($"{channel.Name} is already a hub.")
                    .WithColor(Color.Red)
                    .Build();
                await RespondAsync(embed: embed, ephemeral: true);
            }
        }

        [SlashCommand("remove-hub", "Remove a hub.")]
        public async Task RemoveHubAsync(IVoiceChannel channel)
        {
            if (_hubs.Remove(channel.Id))
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Temporary voice channels")
                    .WithDescription($"Hub {channel.Name} removed.")
                    .WithColor(Color.Green)
                    .Build();
                await RespondAsync(embed: embed, ephemeral: true);
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Temporary voice channels")
                    .WithDescription($"{channel.Name} is not a hub.")
                    .WithColor(Color.Red)
                    .Build();
                await RespondAsync(embed: embed, ephemeral: true);
            }
        }

        [SlashCommand("list-hubs", "List hubs.")]
        public async Task ListHubsAsync()
        {
            var embed = new EmbedBuilder()
                .WithTitle("Temporary voice channels")
                .WithDescription(string.Join("\n", _hubs.Select(id => $"<#{id}>")))
                .WithColor(Color.Green)
                .Build();
            await RespondAsync(embed: embed, ephemeral: true);
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
