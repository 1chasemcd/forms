namespace FormsApi.Common;

internal static class StringExtensions
{
    internal static string CamelCaseToWords(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();
        result.Append(input[0]);

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
                result.Append(' ');

            result.Append(input[i]);
        }

        return result.ToString();
    }
}
