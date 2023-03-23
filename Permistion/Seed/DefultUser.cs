using Microsoft.AspNetCore.Identity;
using Permistion.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Permistion.Seed
{
    public static class DefultUser
    {
        public static async Task AddDefultUser(UserManager<IdentityUser> userManager)
        {

            //new User Detitles
            var newUser = new IdentityUser { 
            UserName = "user@gmail.com",
             Email = "user@gmil.com",
             EmailConfirmed=true
            
            };

            //Make sure that user not exit befor

            var user = await userManager.FindByEmailAsync(newUser.Email);

            //if not exit Create it 

            if (user == null)
            {
                //create it with userName , Password
                await userManager.CreateAsync(newUser ,"Ap@123456");

                //After Created it , it will assign the role to it 
                await userManager.AddToRoleAsync(newUser ,Roles.User.ToString());

            }

        }

        //Making fun to superAdmin and assign all of rules to it 
        public static async Task AddDefultSuperAdmin(UserManager<IdentityUser> userManager , RoleManager<IdentityRole> roleManager)
        {

            //new User Detitles
            var newUser = new IdentityUser
            {
                UserName = "SuperAdmin@gmail.com",
                Email = "SuperAdmin@gmil.com",
                EmailConfirmed = true

            };

            //Make sure that user not exit befor

            var user = await userManager.FindByEmailAsync(newUser.Email);

            //if not exit Create it 

            if (user == null)
            {
                //create it with userName , Password
                await userManager.CreateAsync(newUser, "Ap@123456");

                //After Created it , it will assign All the Roles
                await userManager.AddToRolesAsync(newUser, new List<string> { Roles.User.ToString(), Roles.Admin.ToString(), Roles.SuperAdmin.ToString() });

                //Add permision to this user

                await roleManager.CliamforSuperAdmin();

            }

        }

        //Find Admin Roles
        public static async Task CliamforSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());
            //Add permisition to Role
            await roleManager.AddPermistionCliam(superAdmin, "Products");
        }


        //Add permistion to spsefic user
        //this here to ignor parameter when invoke the function , Refliction methodes
         public static async Task AddPermistionCliam(this RoleManager<IdentityRole> roleManager ,IdentityRole role ,string Module)
         {
            //Get All Cliam Assoited with this Role
            var allCliams = await roleManager.GetClaimsAsync(role);

            //Get the permission for the module parameter, we genrate in class constant

            var allPermissions = ModelPermistion.GenratePrimtionList(Module);

            //loop in the list of allPermissions and added it to cliam
            foreach (var Permission in allPermissions)
            {
                //Make sure that permission not found in the cliam , then added to it
                //type => type of it is Permision   , value = Permission in foreach
                if(!allCliams.Any( x => x.Type == "Permission" &&  x.Value == Permission))
                {
                    //not found then i added to cliam
                    await roleManager.AddClaimAsync(role, new Claim("Permission" , Permission));
                }
            }


        }



    }
}
