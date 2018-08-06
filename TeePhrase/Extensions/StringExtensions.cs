namespace TeePhrase.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateTo(this string input, int truncateLength, bool addEllipsis = true)
        {
            // SOURCE: https://codereview.stackexchange.com/questions/85688/string-extension-method-that-truncates-at-sentence-punctuation
            // Check to see if we even need to bother truncating
            if (string.IsNullOrWhiteSpace(input) || input.Length <= truncateLength)
                return input;

            // copy the input
            var temp = input;

            // Define a list of characters that we can safely break on
            char[] anyOf = { ' ' };

            // LastIndexOfAny starts at the max position works backwards thru' the string
            var truncatePosition = temp.LastIndexOfAny(anyOf, truncateLength);

            // no appropriate place to truncate. 
            if (truncatePosition == -1)
            {
                // Return original string but you could fall back to splitting on last space
                return temp;
            }
            temp = temp.Substring(0, truncatePosition + (addEllipsis ? 0 : 1));
            return addEllipsis
                ? temp + "..."
                : temp;
        }
    }
}
