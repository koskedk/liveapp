using LiveApp.Api;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var liveAuthSetting =builder.Configuration.GetSection(LiveAuthSetting.Key).Get<LiveAuthSetting>();
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
        
        Flows = new OpenApiOAuthFlows
        {
            ClientCredentials = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                Scopes = liveAuthSetting.Scopes
            }
        }
        /*
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{liveAuthSetting.Authority}/connect/authorize"),
                TokenUrl = new Uri($"{liveAuthSetting.Authority}/connect/token"),
                Scopes =liveAuthSetting.Scopes
            }
        }
        */
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>();
} );


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();