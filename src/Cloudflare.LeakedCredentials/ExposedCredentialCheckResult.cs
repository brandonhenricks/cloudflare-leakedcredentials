namespace Cloudflare.LeakedCredentials.Origin
{
    /// <summary>
    /// Represents the result of an exposed credential check from Cloudflare.
    /// </summary>
    public enum ExposedCredentialCheckResult
    {
        /// <summary>
        /// No credential check performed or header not present.
        /// </summary>
        None = 0,

        /// <summary>
        /// Credentials were checked and no exposure detected.
        /// </summary>
        NoExposure = 1,

        /// <summary>
        /// Credentials were exposed in a data breach.
        /// </summary>
        Exposed = 2,

        /// <summary>
        /// Credentials were potentially exposed (low confidence).
        /// </summary>
        PotentiallyExposed = 3,

        /// <summary>
        /// Error occurred during credential check.
        /// </summary>
        Error = 4
    }
}
