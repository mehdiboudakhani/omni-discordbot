namespace Omni.Utils
{
    /// <summary>
    /// Provides strongly-typed access to required environment variables used by the application.
    /// </summary>
    /// <remarks>
    /// This helper class centralizes the retrieval of environment variables and ensures that all required configuration keys are present at runtime.
    /// If a variable is missing, an exception will be thrown to prevent misconfigured execution.
    /// </remarks>
    internal class EnvironmentVariableHelper
    {
        private const string DISCORD_BOT_TOKEN = "OMNI_DISCORD_BOT_TOKEN";
        private const string DISCORD_SERVER_IDENTIFIER = "OMNI_DISCORD_SERVER_IDENTIFIER";
        private const string GITHUB_TOKEN = "OMNI_GITHUB_TOKEN";
        private const string GEMINI_API_KEY = "OMNI_GEMINI_API_KEY";

        /// <summary>
        /// Gets the Discord bot token used to authenticate the bot client.
        /// </summary>
        public static string DiscordBotToken => Get(DISCORD_BOT_TOKEN);

        /// <summary>
        /// Gets the Discord server identifier where the bot is registered.
        /// </summary>
        public static string DiscordServerIdentifier => Get(DISCORD_SERVER_IDENTIFIER);

        /// <summary>
        /// Gets the GitHub personal access token used to authenticate requests to the GitHub API.
        /// </summary>
        public static string GithubToken => Get(GITHUB_TOKEN);

        /// <summary>
        /// Gets the Gemini API key used to authenticate requests to the Gemini API. 
        /// </summary>
        public static string GeminiApiKey => Get(GEMINI_API_KEY);

        /// <summary>
        /// Retrieves the value of the specified environment variable.
        /// </summary>
        /// <param name="key">
        /// The name of the environment variable to retrieve.
        /// </param>
        /// <returns>
        /// The value of the environment variable.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the environment variable is missing or its value is null/empty.
        /// </exception>
        private static string Get(string key)
        {
            var environmentVariable = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrWhiteSpace(environmentVariable))
                throw new Exception($"Environment variable '{key}' is missing.");
            return environmentVariable;         
        }
    }
}
