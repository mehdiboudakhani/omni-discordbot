namespace Omni.Utils
{
    internal class EnvironmentVariableHelper
    {
        private const string DISCORD_BOT_TOKEN = "OMNI_DISCORD_BOT_TOKEN";
        private const string DISCORD_SERVER_IDENTIFIER = "OMNI_DISCORD_SERVER_IDENTIFIER";
        private const string GITHUB_TOKEN = "OMNI_GITHUB_TOKEN";
        private const string GEMINI_API_KEY = "OMNI_GEMINI_API_KEY";

        public static string DiscordBotToken => Get(DISCORD_BOT_TOKEN);

        public static string DiscordServerIdentifier => Get(DISCORD_SERVER_IDENTIFIER);

        public static string GithubToken => Get(GITHUB_TOKEN);

        public static string GeminiApiKey => Get(GEMINI_API_KEY);

        private static string Get(string key)
        {
            var environmentVariable = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(environmentVariable))
                throw new Exception($"Environment variable '{key}' is missing.");
            return environmentVariable;         
        }
    }
}
