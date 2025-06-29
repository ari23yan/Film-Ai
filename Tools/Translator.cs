using GoogleTranslateFreeApi;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Film_Ai.Tools
{
    public static class Translator
    {
        public static async Task<string?> Translate(string? text)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(text)) return null;
                string? from = "en"; string? to = "fa";
                string encodedText = Uri.EscapeDataString(text);

                string url = $"https://api.mymemory.translated.net/get?q={encodedText}&langpair={from}|{to}";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var jsonObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonResponse);
                    if (jsonObject != null && jsonObject.TryGetValue("responseData", out JsonElement responseData) &&
                        jsonObject.TryGetValue("responseStatus", out JsonElement responseStatus))
                    {
                        if ((responseStatus.ValueKind == JsonValueKind.Number && responseStatus.GetInt32() == 200) ||
                            (responseStatus.ValueKind == JsonValueKind.String && responseStatus.GetString() == "200"))
                        {
                            if (responseData.ValueKind == JsonValueKind.Object &&
                                responseData.TryGetProperty("translatedText", out JsonElement translatedText))
                            {
                                return translatedText.GetString();
                            }
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
