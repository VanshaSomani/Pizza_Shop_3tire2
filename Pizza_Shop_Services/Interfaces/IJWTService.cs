using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Pizza_Shop_Services.Interfaces;

public interface IJWTService
{
    public string GenerateJwtToken(string email , string pass , string role);

    public string GetClaimValue(string token , string claimtype);

    public ClaimsPrincipal? GetClaimFromToken(string token);
}
