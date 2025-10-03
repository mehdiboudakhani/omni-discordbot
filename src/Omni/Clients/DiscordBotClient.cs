namespace Omni.Clients
{
    /// <summary>
    /// Provides the core logic for initializing and managing the Omni Discord bot client.
    /// </summary>
    /// <param name="discordSocketClient">
    /// The Discord socket client instance used to connect to the Discord Gateway and receive events.
    /// </param>
    /// <param name="serviceProvider">
    /// The application's dependency injection container, providing access to registered services.
    /// </param>
    /// <param name="interactionService">
    /// The interaction service responsible for handling and executing slash commands and other interactions.
    /// </param>
    class DiscordBotClient(DiscordSocketClient discordSocketClient, IServiceProvider serviceProvider, InteractionService interactionService, ISecretProvider secretProvider)
    {
        /// <summary>
        /// Initializes the Discord bot, loads modules and starts the client.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous initialization process.
        /// </returns>
        public async Task InitializeAsync()
        {
            await interactionService.AddModulesAsync(typeof(GithubModule).Assembly, serviceProvider);
            discordSocketClient.InteractionCreated += OnInteractionCreatedAsync;
            discordSocketClient.Ready += OnReadyAsync;
            await discordSocketClient.LoginAsync(TokenType.Bot, secretProvider.DiscordBotToken);
            await discordSocketClient.StartAsync();
        }

        /// <summary>
        /// Event handler that executes when a new interaction is created.
        /// Routes the interaction to the <see cref="InteractionService"/> for execution.
        /// </summary>
        /// <param name="socketInteraction">
        /// The interaction received from Discord.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous handling of the interaction.
        /// </returns>
        private async Task OnInteractionCreatedAsync(SocketInteraction socketInteraction) =>
            await interactionService.ExecuteCommandAsync(new SocketInteractionContext(discordSocketClient, socketInteraction), serviceProvider);

        /// <summary>
        /// Event handler that executes when the Discord client is ready.
        /// Registers all slash commands to the configured guild.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous command registration process.
        /// </returns>
        private async Task OnReadyAsync() =>
                await interactionService.RegisterCommandsToGuildAsync(ulong.Parse(secretProvider.DiscordServerIdentifier));
    }
}
