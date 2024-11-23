using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace PadelChampAPI.Data;

public interface IJwtTokenGenerator
{
    public string GenerateToken(List<Claim> claims, IConfiguration config);
}
