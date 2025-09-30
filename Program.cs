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
            services.AddHttpClient("GithubHttpClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://api.github.com/");
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Omni");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Environment.GetEnvironmentVariable("OMNI_GITHUB_TOKEN")!);
            });
            services.AddHttpClient("GeminiHttpClient", httpClient => httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/"));
            services.AddSingleton<DiscordBotClient>();
            return services.BuildServiceProvider();
        }
    }
}