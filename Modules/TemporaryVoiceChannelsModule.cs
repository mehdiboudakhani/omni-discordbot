namespace Omni.Modules
{
    [Group("temporary-voice-channels", "Temporary voice channels management.")]
    public class TemporaryVoiceChannelsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private static readonly HashSet<ulong> _hubChannels = new();
        private static readonly Dictionary<ulong, ulong> _temporaryChannels = new();
        private readonly DiscordSocketClient _discordSocketClient;

        public TemporaryVoiceChannelsModule(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _discordSocketClient.UserVoiceStateUpdated += HandleVoiceStateUpdatedAsync;
        }

        [SlashCommand("add-hub", "Add a hub voice channel.")]
        public async Task AddHubAsync(IVoiceChannel hubChannel)
        {
            if (_hubChannels.Add(hubChannel.Id))
                await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsAdd(hubChannel), ephemeral: true);
            else
                await RespondAsync(embed: EmbedHelper.Error($"{hubChannel.Name} is already a hub."), ephemeral: true);
        }

        [SlashCommand("remove-hub", "Remove a hub voice channel")]
        public async Task RemoveHubAsync(IVoiceChannel hubChannel)
        {
            if (_hubChannels.Remove(hubChannel.Id))
                await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsRemove(hubChannel), ephemeral: true);
            else
                await RespondAsync(embed: EmbedHelper.Error($"{hubChannel.Name} was not a hub."), ephemeral: true);
        }

        [SlashCommand("list-hubs", "List all configured hub channels.")]
        public async Task ListHubAsync()
        {
            await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsList(_hubChannels), ephemeral: true);
        }


        private async Task HandleVoiceStateUpdatedAsync(SocketUser user, SocketVoiceState before, SocketVoiceState after)
        {
            if (user is not SocketGuildUser guildUser) return;
            if (after.VoiceChannel != null && _hubChannels.Contains(after.VoiceChannel.Id))
            {
                if (_temporaryChannels.ContainsKey(guildUser.Id))
                    return;
                var category = after.VoiceChannel.Category;
                var tempChannel = await guildUser.Guild.CreateVoiceChannelAsync($"{guildUser.Username}'s VC", props =>
                {
                    if (category != null) props.CategoryId = category.Id;
                });
                _temporaryChannels[guildUser.Id] = tempChannel.Id;
                await guildUser.ModifyAsync(x => x.Channel = tempChannel);
            }
            if (before.VoiceChannel != null && _temporaryChannels.Values.Contains(before.VoiceChannel.Id))
            {
                var tempChannel = before.VoiceChannel;
                if (tempChannel.ConnectedUsers.Count == 0)
                {
                    await tempChannel.DeleteAsync();
                    var ownerEntry = _temporaryChannels.FirstOrDefault(kvp => kvp.Value == tempChannel.Id);
                    if (!ownerEntry.Equals(default(KeyValuePair<ulong, ulong>)))
                        _temporaryChannels.Remove(ownerEntry.Key);
                }
            }
        }

    }
}
