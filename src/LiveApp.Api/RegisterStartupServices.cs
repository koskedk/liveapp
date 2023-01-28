using System;
using LiveApp.Api.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LiveApp.Api
{
    public static class RegisterStartupServices
    {
        public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
        {
            var liveAuthSetting = builder.Configuration.GetSection(LiveAuthSetting.Key).Get<LiveAuthSetting>();
            builder.Services.AddSingleton(liveAuthSetting);

            builder.Services.Configure<LiveAuthSetting>(builder.Configuration.GetSection(LiveAuthSetting.Key));
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = liveAuthSetting.Authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = SetUpFlow(liveAuthSetting)
                });
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            return builder;
        }

        private static OpenApiOAuthFlows SetUpFlow(LiveAuthSetting liveAuthSetting)
        {
            if (liveAuthSetting.Flow == "AuthorizationCode")
                return new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                        TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                        Scopes = liveAuthSetting.Scopes
                    }
                };

            return new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                    TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                    Scopes = liveAuthSetting.Scopes
                }
            };
        }
    }
}