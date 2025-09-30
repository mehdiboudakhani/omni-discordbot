namespace Omni
{
    class Program
    {
        static async Task Main()
        {
            await ConfigureServices().GetRequiredService<DiscordBotClient>().InitializeAsync();
            await Task.Delay(-1);
        }

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
            services.AddHttpClient("GithubHttpClient", HttpClientConfigurator.ConfigureGithub);
            services.AddHttpClient("GeminiHttpClient", HttpClientConfigurator.ConfigureGemini);
            services.AddSingleton<DiscordBotClient>();
            return services.BuildServiceProvider();
        }
    }
}