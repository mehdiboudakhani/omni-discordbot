namespace Omni.Configuration
{
    public class SecretProvider : ISecretProvider
    {
        private const string DISCORD_BOT_TOKEN = "OMNI_DISCORD_BOT_TOKEN";
        private const string DISCORD_SERVER_IDENTIFIER = "OMNI_DISCORD_SERVER_IDENTIFIER";
        private const string GITHUB_TOKEN = "OMNI_GITHUB_TOKEN";
        private const string GEMINI_API_KEY = "OMNI_GEMINI_API_KEY";

        public string DiscordBotToken => Get(DISCORD_BOT_TOKEN);

        public string DiscordServerIdentifier => Get(DISCORD_SERVER_IDENTIFIER);

        public string GithubToken => Get(GITHUB_TOKEN);

        public string GeminiApiKey => Get(GEMINI_API_KEY);

        private string Get(string key)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"Secret '{key}' is missing.");
            return value!;
        }
    }
}
