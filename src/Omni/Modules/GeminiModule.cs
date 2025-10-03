namespace Omni.Modules
{
    /// <summary>
    /// Provides interaction commands for communicating with the Google Gemini API.
    /// </summary>
    [Group("gemini", "Interact with Gemini.")]
    public class GeminiModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeminiModule"/> class.
        /// Configures the HTTP client used to send requests to the Gemini API and retrieves the API key from environment variables.
        /// </summary>
        /// <param name="httpClientFactory">
        /// The factory used to create named <see cref="HttpClient"/> instances.
        /// </param>
        public GeminiModule(IHttpClientProvider httpClientProvider, ISecretProvider secretProvider)
        {
            _httpClient = httpClientProvider.GetGeminiHttpClient();
            _apiKey = secretProvider.GeminiApiKey;
        }

        /// <summary>
        /// Asks a question to the Gemini model and returns its response.
        /// </summary>
        /// <param name="prompt">
        /// The input text prompt to send to Gemini.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// Sends an embedded response containing the model's answer or an error message.
        /// </returns>
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
