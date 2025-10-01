namespace Omni.Common
{
    /// <summary>
    /// Provides centralized configuration methods for <see cref="HttpClient"/> instances used by the Omni application.
    /// </summary>
    internal class HttpClientConfigurator
    {
        private const string GithubApiUrl = "https://api.github.com/";
        private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/";
        private const string UserAgent = "Omni";

        /// <summary>
        /// Configures an <see cref="HttpClient"/> instance for GitHub API requests.
        /// Sets the base address, user agent, and authentication header.
        /// </summary>
        /// <param name="httpClient">
        /// The <see cref="HttpClient"/> to configure for GitHub requests.
        /// </param>
        /// <remarks>
        /// The authentication token is retrieved from <see cref="EnvironmentVariableHelper.GithubToken"/>.
        /// Ensure that the <c>OMNI_GITHUB_TOKEN</c> environment variable is set before calling this method.
        /// </remarks>
        public static void ConfigureGithub(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(GithubApiUrl);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(EnvironmentVariableHelper.GithubToken);
        }

        /// <summary>
        /// Configures an <see cref="HttpClient"/> instance for Gemini API requests.
        /// Sets the base address only.
        /// </summary>
        /// <param name="httpClient">
        /// The <see cref="HttpClient"/> to configure for Gemini requests.
        /// </param>
        public static void ConfigureGemini(HttpClient httpClient) => httpClient.BaseAddress = new Uri(GeminiApiUrl);
        
    }
}
