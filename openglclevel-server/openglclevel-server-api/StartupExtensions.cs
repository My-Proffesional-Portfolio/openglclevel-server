using Microsoft.OpenApi.Models;
using openglclevel_server_api.UtilControllers;
using openglclevel_server_backend.Services;
using openglclevel_server_infrastructure.Repositories.Interfaces;
using openglclevel_server_infrastructure.Repositories;
using openglclevel_server_infrastructure.Services;
using openglclevel_server_security.Decryptor;
using openglclevel_server_security.Encryptor;
using openglclevel_server_security.TokenManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace openglclevel_server_api
{
    public static class StartupExtensions
    {
        public static void AddCustomSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        }

        public static void InjectServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMealItemService, MealItemService>();
            services.AddTransient<IMealEventService, MealEventService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IMealItemRepository, MealItemRepository>();
            services.AddTransient<IMealEventRepository, MealEventRepository>();
            services.AddTransient<IMealEventItemsRepository, MealEventItemsRepository>();

            services.AddTransient<EncryptorEngine>();
            services.AddTransient<DecryptorEngine>();
            services.AddTransient<TokenHandlerEngine>();
            services.AddTransient<ControllerUtilities>();
        }

        public static void AddCustomAuthentication(this IServiceCollection services, string JWTKey)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "openglclevel-app",
                    ValidAudience = "openglclevel-app",
                    IssuerSigningKey = mySecurityKey,
                };
            });
        }
    }
}
