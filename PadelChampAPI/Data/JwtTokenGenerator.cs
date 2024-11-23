using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PadelChampAPI.Data;

namespace PadelChampAPI.Data;


public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(List<Claim> claims, IConfiguration config)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var jwtIssuer = _jwtSettings.Issuer;

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtIssuer,
            expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials: credentials,
            claims: claims
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }
}

public class JwtSettings
{
    public string Key { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpiryInMinutes { get; init; }

}


