using System.Net.Http.Json;
using System.Text;

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
            _apiKey = Environment.GetEnvironmentVariable("OMNI_GEMINI_API_KEY") ?? throw new Exception("Gemini API key not found.");
        }

        [SlashCommand("ask", "Ask a question to Gemini.")]
        public async Task AskAsync(string prompt)
        {
            await DeferAsync();
            try
            {
                var payload = JsonSerializer.Serialize(new
                {
                    contents = new[]
                    {
                        new {
                            parts = new[] { new { text = prompt } }
                        }
                    }
                });
                var response = await _httpClient.PostAsync($"v1/models/gemini-2.5-flash:generateContent?key={_apiKey}", new StringContent(payload, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    await FollowupAsync(embed: EmbedHelper.Error("Unable to find the requested resource on Gemini."), ephemeral: true);
                    return;
                }
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();
                await FollowupAsync(embed: EmbedHelper.GeminiAsk(prompt, json));
            }
            catch (Exception ex)
            {
                await FollowupAsync(embed: EmbedHelper.Error("A technical problem occurred while contacting Gemini."), ephemeral: true);
            }
        }
    }
}
