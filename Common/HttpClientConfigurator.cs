namespace Omni.Common
{
    internal class HttpClientConfigurator
    {
        private const string GithubApiUrl = "https://api.github.com/";
        private const string GeminiApiUrl = "https://generativelanguage.googleapis.com/";
        private const string UserAgent = "Omni";

        public static void ConfigureGithub(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(GithubApiUrl);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(EnvironmentVariableHelper.GithubToken);
        }

        public static void ConfigureGemini(HttpClient httpClient) => httpClient.BaseAddress = new Uri(GeminiApiUrl);
        
    }
}
