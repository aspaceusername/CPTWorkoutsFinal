using CPTWorkouts.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Aulas.Data
{
    internal static class DbInitializerExtension
    {
        public static async Task<IApplicationBuilder> UseItToSeedSqlServer(this IApplicationBuilder app)
        {
            ArgumentNullException.ThrowIfNull(app, nameof(app));

            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await DbInitializer.Initialize(context, userManager, roleManager);
            }
            catch (Exception ex)
            {
                // Handle the exception (log it, rethrow it, etc.)
                throw;
            }

            return app;
        }
    }
}
