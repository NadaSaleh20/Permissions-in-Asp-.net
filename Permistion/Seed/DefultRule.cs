using Microsoft.AspNetCore.Identity;
using Permistion.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permistion.Seed
{
    public static class DefultRule
    {
        

        public static async Task AddRole(RoleManager<IdentityRole> roleManager)
        {
            //Check of the Role not exit in the file, then added it
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            }
         
        }
    }
}
