using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace LiveApp.Api
{
    public static class RegisterStartupMiddlewares
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}