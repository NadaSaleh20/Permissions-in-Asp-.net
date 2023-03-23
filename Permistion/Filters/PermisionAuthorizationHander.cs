using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permistion.Filters
{
    public class PermisionAuthorizationHander : AuthorizationHandler<PermisonsRequrments>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermisonsRequrments requirement)
        {
            //Make Sure That User exit in the application

            if (context.User == null)
                return;

            //We Want to Know if he can acees to given permision
            //Permission  => we detrmine it when Make CLiam
            //Value come from requrment class


            var CanAcees = context.User.Claims.Any(x => x.Type == "Permission" && 
                                                   x.Value == requirement.Permistions &&
                                                   x.Issuer =="LOCAL AUTHORITY");
      

            if (CanAcees)
            {
                //finally if it True User Can Open the Page with permition Have
                context.Succeed(requirement);
                return;
            }
        }
    }
}
