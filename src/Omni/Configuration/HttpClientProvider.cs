namespace Omni.Configuration
{
    public interface IHttpClientProvider
    {
        HttpClient GetGithubHttpClient();
        HttpClient GetGeminiHttpClient();
    }

    public class HttpClientProvider(ISecretProvider secretProvider) : IHttpClientProvider
    {
        private const string GithubBaseUrl = "https://api.github.com/";
        private const string GeminiBaseUrl = "https://generativelanguage.googleapis.com/";
        private const string UserAgent = "Omni";

        public HttpClient GetGeminiHttpClient()
        {
            var httpClient = new HttpClient();
            Configure(httpClient, GeminiBaseUrl, null, null);
            return httpClient;
        }

        public HttpClient GetGithubHttpClient()
        {
            var httpClient = new HttpClient();
            Configure(httpClient, GithubBaseUrl, UserAgent, secretProvider.GithubToken);
            return httpClient;
        }

        private void Configure(HttpClient httpClient, string baseUrl, string? userAgent, string? authToken)
        {
            httpClient.BaseAddress = new Uri(baseUrl);
            if (!string.IsNullOrWhiteSpace(userAgent))
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            if (!string.IsNullOrWhiteSpace(authToken))
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
    }
}
