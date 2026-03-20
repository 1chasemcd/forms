namespace FormsApi.Common;

internal static class StringExtensions
{
    private static readonly HashSet<string> _notCapitalized = new(StringComparer.OrdinalIgnoreCase)
    {
        "a","an","the",
        "and","but","or","nor","for","so","yet",
        "as","at","by","in","of","off","on","per","to","up","via",
        "from","into","onto","upon","with","than","till","over","out"
    };
    internal static string CamelCaseToWords(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        List<string> words = [];

        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
                words.Add("");

            words[words.Count - 1] += input[i];
        }

        if (words.Count <= 2) return string.Join(' ', words);

        // Fix capitalization on words that are not the first or last word
        IEnumerable<string> wordsCorrected = words.Skip(1).SkipLast(1).Select(
            word => _notCapitalized.Contains(word.ToLower()) ? word.ToLower() : word);

        return words.First() + " " + string.Join(' ', wordsCorrected) + " " + words.Last();
    }
}
