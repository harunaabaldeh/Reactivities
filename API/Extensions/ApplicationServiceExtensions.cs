using Application.Activities;
using Application.Core;
using Application.Interfaces;
using Domain;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static async void AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DafaultConnection"));
            });

            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            }));

            builder.Services.AddMediatR(typeof(List.Handler).Assembly);
            builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            builder.Services.AddScoped<IUserAccessor, UserAccessor>();


        }
    }

}