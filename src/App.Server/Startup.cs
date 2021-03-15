using System;
using System.Text;
using App.Server.Database;
using App.Server.Database.Storage;
using App.Server.Hubs;
using App.Server.Options;
using App.Server.Services;
using App.Server.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace App.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options => options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                                ValidateAudience = false,
                                ValidateIssuer = false,
                                ValidateActor = false,
                                ValidateLifetime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Auth:SecurityKey")))
                            });
            
            services.AddMvcCore(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter());
            });
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny", builder => builder
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => true));
            });
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDataStorage, DataStorage>();
            
            services.AddDbContext<DbChatContext>(opt => opt.UseInMemoryDatabase("ChatDatabase"));

            services.Configure<ChatOptions>(Configuration.GetSection("Chat"));
            services.Configure<AuthOptions>(Configuration.GetSection("Auth"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseCors("AllowAny");

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(opt => 
            {
                opt.MapControllers();
                opt.MapHub<ChatHub>("/signalr/chat");
            });
        }
    }
}