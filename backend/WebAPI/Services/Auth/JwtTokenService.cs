using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utility;
using WebAPI.Options;

namespace WebAPI.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOption jwtOption = new();

    public JwtTokenService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
        var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOption.ExpireMin),
            signingCredentials: signInCredentials
        );

        // Write the token to a string
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return tokenString;
    }

    public string GenerateRefreshToken(string email)
    {
        long epochTime = DateTimeUtils.TimeInEpoch();

        string tokenData = $"{email}:{epochTime}";

        // Convert the combined data to a byte array
        var tokenBytes = Encoding.UTF8.GetBytes(tokenData);

        // Generate a hash of the combined data using HashData
        var hashBytes = SHA256.HashData(tokenBytes);

        // Convert the hash bytes to a Base64 string
        return Convert.ToBase64String(hashBytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            //ValidateAudience = false,
            //ValidateIssuer = false,
            // -- OR --
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = jwtOption.Issuer,
            ValidAudience = jwtOption.Audience,
            ValidateLifetime = false, // Do not validate the token's lifetime (allow expired tokens)
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            //ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // Validate the token and get the principal (user information)
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

        // Check if the token is valid and uses the expected algorithm
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        // Return the principal (user information) extracted from the token
        return principal;
    }
}
