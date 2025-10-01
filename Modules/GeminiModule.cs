namespace Omni.Modules
{
    [Group("gemini", "Interact with Gemini.")]
    public class GeminiModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiModule(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GeminiHttpClient");
            _apiKey = EnvironmentVariableHelper.GeminiApiKey;
        }

        [SlashCommand("ask", "Ask a question to Gemini.")]
        public async Task AskAsync(string prompt)
        {
            await DeferAsync();
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"v1/models/gemini-2.5-flash:generateContent?key={_apiKey}", new { contents = new[] { new { parts = new[] { new { text = prompt } } } } });
                if (!response.IsSuccessStatusCode)
                {
                    await FollowupAsync(embed: EmbedHelper.Error("Unable to find the requested resource on Gemini."), ephemeral: true);
                    return;
                }
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                await FollowupAsync(embed: EmbedHelper.GeminiAsk(prompt, json));
            }
            catch (Exception)
            {
                await FollowupAsync(embed: EmbedHelper.Error("A technical problem occurred while contacting Gemini."), ephemeral: true);
            }
        }
    }
}
