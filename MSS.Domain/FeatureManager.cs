using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSS.Database.AppDbContextModels;


namespace MSS.Domain
{
    public static class FeatureManager
    {
        public static WebApplicationBuilder AddDomain(this WebApplicationBuilder builder)
        {
            // Features
           // builder.Services.AddScoped<IMeetingService, MeetingService>();


            return builder;
        }

        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }
    }
}
