using LiveApp.Api;
using Microsoft.AspNetCore.Builder;

WebApplication.CreateBuilder(args)
    .RegisterServices()
    .Build()
    .SetupMiddleware()
    .Run();