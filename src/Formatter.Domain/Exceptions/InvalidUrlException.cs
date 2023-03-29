using System.Text.RegularExpressions;

namespace CandidateTesting.JoaoMatheus.AgoraFormatter.Domain.Exceptions
{
    public partial class InvalidUrlException : Exception
    {
        private const string DefaultErrorMessage = "Invalid URL";
        private const string UrlRegexPattern = "^https?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$";

        public InvalidUrlException(string message = DefaultErrorMessage)
            : base(message)
        {
        }

        public static void ThrowIfInvalid(
            string url,
            string message = DefaultErrorMessage)
        {           
            if (!Regex.IsMatch(url, UrlRegexPattern))
                throw new InvalidUrlException(message: message);
        }
    }
}
