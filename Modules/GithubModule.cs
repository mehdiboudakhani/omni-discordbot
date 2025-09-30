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
                await RespondAsync(embed: EmbedHelper.ErrorEmbed($"Unable to find the GitHub user {username}."), ephemeral: true);
                return;
            }
            var content = await response.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(content);
            var user = jsonDocument.RootElement;

            var name = user.TryGetProperty("name", out var nameProp) ? nameProp.GetString() : null;
            var bio = user.TryGetProperty("bio", out var bioProp) ? bioProp.GetString() : null;

            var embed = new EmbedBuilder()
                .WithTitle($"GitHub profile : {user.GetProperty("login").GetString()}")
                .WithUrl(user.GetProperty("html_url").GetString())
                .WithThumbnailUrl(user.GetProperty("avatar_url").GetString())
                .WithColor(Color.Green);
            if (!string.IsNullOrWhiteSpace(name))
                embed.AddField("Nom", name, true);
            if (!string.IsNullOrWhiteSpace(bio))
                embed.AddField("Bio", bio, true);
            await RespondAsync(embed: embed.Build());
        }
    }
}
