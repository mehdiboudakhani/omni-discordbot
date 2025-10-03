namespace Omni.Configuration
{
    public interface IHttpClientProvider
    {
        HttpClient GetGithubHttpClient();
        HttpClient GetGeminiHttpClient();
    }
}
