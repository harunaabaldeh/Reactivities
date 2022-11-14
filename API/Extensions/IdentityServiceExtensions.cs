using System.Text;
using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static void AddIdentityServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityCore<AppUser>(opt =>
          {
              opt.Password.RequireNonAlphanumeric = false;
          })
            .AddEntityFrameworkStores<DataContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret key"));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(opt =>
                            {
                                opt.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = key,
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };
                            });
            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });

            builder.Services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            builder.Services.AddScoped<TokenService>();


        }
    }
}