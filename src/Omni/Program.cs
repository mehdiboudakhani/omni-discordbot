namespace Omni
{
    /// <summary>
    /// Main entry point of the Omni application.
    /// Configures services and starts the Discord Client.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Asynchronous entry method.
        /// Builds and configures the service provider, then launches the <see cref="DiscordBotClient"/>
        /// </summary>
        static async Task Main()
        {
            await ConfigureServices().GetRequiredService<DiscordBotClient>().InitializeAsync();
            await Task.Delay(-1);
        }

        /// <summary>
        /// Configures the application's dependency injection container.
        /// Initializes Discord clients, HTTP clients, and core services.
        /// </summary>
        /// <returns>
        /// A fully configured <see cref="ServiceProvider"/> for dependency resolution.
        /// </returns>
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged
                                 | GatewayIntents.GuildMembers
                                 | GatewayIntents.GuildVoiceStates
                                 | GatewayIntents.MessageContent
            }));
            services.AddSingleton(serviceProvider => new InteractionService(serviceProvider.GetRequiredService<DiscordSocketClient>()));
            services.AddSingleton<ISecretProvider, SecretProvider>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();
            services.AddSingleton<DiscordBotClient>();
            return services.BuildServiceProvider();
        }
    }
}