namespace Omni.Modules
{
    /// <summary>
    /// Provides commands and event handling for temporary voice channels management.
    /// Allows configuration of "hub" channels where users can join to automatically create a temporary voice channel.
    /// </summary>
    [Group("temporary-voice-channels", "Temporary voice channels management.")]
    public class TemporaryVoiceChannelsModule : InteractionModuleBase<SocketInteractionContext>
    {
        private static readonly HashSet<ulong> _hubChannels = new();
        private static readonly Dictionary<ulong, ulong> _temporaryChannels = new();
        private readonly DiscordSocketClient _discordSocketClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryVoiceChannelsModule"/> class.
        /// Subscribes to the <see cref="DiscordSocketClient.UserVoiceStateUpdated"/> event to handle voice state changes.
        /// </summary>
        /// <param name="discordSocketClient">
        /// The Discord client instance used to track voice state updates.
        /// </param>
        public TemporaryVoiceChannelsModule(DiscordSocketClient discordSocketClient)
        {
            _discordSocketClient = discordSocketClient;
            _discordSocketClient.UserVoiceStateUpdated += HandleVoiceStateUpdatedAsync;
        }

        /// <summary>
        /// Registers a voice channel as a hub for creating temporary voice channels.
        /// </summary>
        /// <param name="hubChannel">
        /// The channel to register as a hub.
        /// </param>
        /// <returns>
        /// A task representing the asynchrnous operation, sending an embed indicating success or failure.
        /// </returns>
        [SlashCommand("add-hub", "Add a hub voice channel.")]
        public async Task AddHubAsync(IVoiceChannel hubChannel)
        {
            if (_hubChannels.Add(hubChannel.Id))
                await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsAdd(hubChannel), ephemeral: true);
            else
                await RespondAsync(embed: EmbedHelper.Error($"{hubChannel.Name} is already a hub."), ephemeral: true);
        }

        /// <summary>
        /// Removes a hub voice channel from the configuration.
        /// </summary>
        /// <param name="hubChannel">
        /// The channel to remove as a hub.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation, sending an embed indicating success or failure.
        /// </returns>
        [SlashCommand("remove-hub", "Remove a hub voice channel")]
        public async Task RemoveHubAsync(IVoiceChannel hubChannel)
        {
            if (_hubChannels.Remove(hubChannel.Id))
                await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsRemove(hubChannel), ephemeral: true);
            else
                await RespondAsync(embed: EmbedHelper.Error($"{hubChannel.Name} was not a hub."), ephemeral: true);
        }

        /// <summary>
        /// Lists all currently registered hub channels.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation, sending an embed listing all hub channels.
        /// </returns>
        [SlashCommand("list-hubs", "List all configured hub channels.")]
        public async Task ListHubAsync()
        {
            await RespondAsync(embed: EmbedHelper.TemporaryVoiceChannelsList(_hubChannels), ephemeral: true);
        }

        /// <summary>
        /// Handles voice state updates for users.
        /// </summary>
        /// <param name="user">
        /// The user whose voice state changed.
        /// </param>
        /// <param name="before">
        /// The user's previous voice state.
        /// </param>
        /// <param name="after">
        /// The user's current voice state.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous event handling operation.
        /// </returns>
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
