using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql.Replication;

namespace Pizza_Shop_Services.Services;

public static class JWTServiceExtension
{
    public static void AddJWTAuthentication(this IServiceCollection services , IConfiguration configuration){
        //jwt token configration
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
            options.Events = new JwtBearerEvents{
                //OnMessageReceived run when authentication request is recived
                OnMessageReceived = context => {
                    var accessToken = context.Request.Cookies["jwtToken"];
                    //check token axists
                    if(!string.IsNullOrEmpty(accessToken)){
                        //store token
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }
}
