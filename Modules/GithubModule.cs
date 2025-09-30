namespace Omni.Modules
{
    [Group("github", "Interacting with GitHub.")]
    public class GithubModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;

        public GithubModule(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GithubHttpClient");
        }

        [SlashCommand("profile", "Viewing a GitHub user's profile.")]
        public async Task UserAsync(string username)
        {
            var response = await _httpClient.GetAsync($"users/{username}");
            if (!response.IsSuccessStatusCode)
            {
                await RespondAsync("Error : Unable to find the GitHub user.");
                return;
            }
            var content = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(content);
            var user = jsonDocument.RootElement;
            await RespondAsync("The user " + user.GetProperty("login").GetString() + " was be found.");
        }
    }
}
