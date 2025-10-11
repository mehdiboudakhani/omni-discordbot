namespace Omni.Providers
{
    public class SecretProvider
    {
        public string DiscordBotToken => GetEnvironmentVariable("OMNI_DISCORD_BOT_TOKEN");

        public string DiscordGuildIdentifier => GetEnvironmentVariable("OMNI_DISCORD_GUILD_IDENTIFIER");

        public string GitHubToken => GetEnvironmentVariable("OMNI_GITHUB_TOKEN");

        private string GetEnvironmentVariable(string name) =>
            Environment.GetEnvironmentVariable(name) ?? throw new Exception($"Environment variable '{name}' is missing.");
    }
}
