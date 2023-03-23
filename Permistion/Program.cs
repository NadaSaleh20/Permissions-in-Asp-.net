using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;

namespace Permistion
{
    public class Program
    {
        public async static Task Main(string[] args)
        {

            //inilaize Values 
            var Host = CreateHostBuilder(args).Build();
            using var scope = Host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerProvider>();
            var logger = loggerFactory.CreateLogger("app");

            // 
            try
            {
                
                //paramerter using in the function
                var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManger = services.GetRequiredService<UserManager<IdentityUser>>();

                //Call the fun
                await Seed.DefultRule.AddRole(roleManger);
                await Seed.DefultUser.AddDefultSuperAdmin(userManger, roleManger);
                await Seed.DefultUser.AddDefultUser(userManger);

                logger.LogInformation("Data Seed");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex , "An error Accured while Seeding the data");
            }

            Host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
