using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pizza_Shop_Services.Interfaces;

namespace Pizza_Shop_Services.Services;

public class JWTService : IJWTService
{
    private readonly IConfiguration _configuration;

    public JWTService(IConfiguration configuration){
        _configuration = configuration;
    }
    public string GenerateJwtToken(string email, string pass, string role)
    {
        var claims = new[]
        {
            new Claim("email",email),
            new Claim("pass",pass),
            new Claim("role",role),
            new Claim(ClaimTypes.Role, role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["jwt:Issuer"],
            _configuration["jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["jwt:ExpireInDays"])),
            signingCredentials: cred
        );
         var newToken = new JwtSecurityTokenHandler().WriteToken(token);
        return newToken;
    }

    public ClaimsPrincipal? GetClaimFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var claims = new ClaimsIdentity(jsonToken.Claims);
        return new ClaimsPrincipal(claims);
    }


    public string GetClaimValue(string token, string claimtype)
    {  
        var claimPrinciple = GetClaimFromToken(token);
        var value = claimPrinciple?.FindFirst(claimtype)?.Value;
        return value;
    }
}
