using GoogleTranslateFreeApi;

namespace Film_Ai.Tools
{
    public static class Translator
    {
        public static async Task<string?> Translate(string text, Language from, Language to)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var translator = new GoogleTranslator();

                    var translationResult = await translator.TranslateLiteAsync(text, from, to);
                    return translationResult.MergedTranslation;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
