using System.Security.Claims;

namespace WebAPI.Services.Auth;

public interface IJwtTokenService
{
    /// <summary>
    /// Generates an access token using the provided claims.
    /// <para>
    /// Access tokens are short-lived and are used to access protected resources.
    /// </para>
    /// </summary>
    /// <param name="claims">The claims to include in the token.</param>
    string GenerateAccessToken(IEnumerable<Claim> claims);


    /// <summary>
    /// Refresh tokens are long-lived and are used to obtain new access tokens
    /// <para>
    /// without needing to re-authenticate the user
    /// </para>
    /// </summary>
    string GenerateRefreshToken(string email);

    /// <summary>
    /// Retrieves the ClaimsPrincipal (user information) from an expired token
    /// <para>
    /// Useful for token renewal or validation without requiring the user to re-authenticate
    /// </para>
    /// </summary>
    /// <param name="token"></param>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
