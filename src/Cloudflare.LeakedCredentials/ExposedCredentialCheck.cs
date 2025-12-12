using System;

namespace Cloudflare.LeakedCredentials.Origin
{
    /// <summary>
    /// Provides functionality to parse Cloudflare's exposed credential check header.
    /// </summary>
    public static class ExposedCredentialCheck
    {
        /// <summary>
        /// The name of the Cloudflare exposed credential check header.
        /// </summary>
        public const string HeaderName = "Exposed-Credential-Check";

        /// <summary>
        /// Parses the exposed credential check result from HTTP headers.
        /// </summary>
        /// <param name="getHeader">A function to retrieve header values by name.</param>
        /// <returns>The parsed <see cref="ExposedCredentialCheckResult"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="getHeader"/> is null.</exception>
        public static ExposedCredentialCheckResult Parse(Func<string, string?> getHeader)
        {
            if (getHeader == null)
            {
                throw new ArgumentNullException(nameof(getHeader));
            }

            var headerValue = getHeader(HeaderName);

            if (headerValue == null || string.IsNullOrWhiteSpace(headerValue))
            {
                return ExposedCredentialCheckResult.None;
            }

            var trimmedValue = headerValue.Trim();

            if (!int.TryParse(trimmedValue, out int result))
            {
                return ExposedCredentialCheckResult.None;
            }

            if (Enum.IsDefined(typeof(ExposedCredentialCheckResult), result) && result > 0)
            {
                return (ExposedCredentialCheckResult)result;
            }

            return ExposedCredentialCheckResult.None;
        }
    }
}
