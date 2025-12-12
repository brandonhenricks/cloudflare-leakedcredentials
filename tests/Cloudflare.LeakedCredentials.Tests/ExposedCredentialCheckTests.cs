using System;
using Cloudflare.LeakedCredentials.Origin;
using Xunit;

namespace Cloudflare.LeakedCredentials.Tests
{
    public class ExposedCredentialCheckTests
    {
        [Fact]
        public void Parse_ThrowsWhenDelegateIsNull()
            => Assert.Throws<ArgumentNullException>(() => ExposedCredentialCheck.Parse(null!));

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Parse_NoHeaderValue_ReturnsNone(string? headerValue)
        {
            var result = ExposedCredentialCheck.Parse(name => headerValue);

            Assert.Equal(ExposedCredentialCheckResult.None, result);
        }

        [Theory]
        [InlineData("1", ExposedCredentialCheckResult.NoExposure)]
        [InlineData("2", ExposedCredentialCheckResult.Exposed)]
        [InlineData("3", ExposedCredentialCheckResult.PotentiallyExposed)]
        [InlineData("4", ExposedCredentialCheckResult.Error)]
        [InlineData(" 2 ", ExposedCredentialCheckResult.Exposed)]
        public void Parse_KnownValue_ReturnsResult(string headerValue, ExposedCredentialCheckResult expected)
        {
            var result = ExposedCredentialCheck.Parse(name => name == ExposedCredentialCheck.HeaderName ? headerValue : null);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("abc")]
        [InlineData("0")]
        [InlineData("5")]
        public void Parse_InvalidValue_ReturnsNone(string headerValue)
        {
            var result = ExposedCredentialCheck.Parse(name => name == ExposedCredentialCheck.HeaderName ? headerValue : null);

            Assert.Equal(ExposedCredentialCheckResult.None, result);
        }
    }
}
