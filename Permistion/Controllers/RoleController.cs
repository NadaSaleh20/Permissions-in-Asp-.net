using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Permistion.Constant;
using Permistion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Permistion.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {

            var Rules = await _roleManager.Roles.ToListAsync();
            return View(Rules);
        }
        [HttpPost]
        public async Task<IActionResult> AddRule(RoleViewModel role)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            //check of the role is exit
            var exitRole = await _roleManager.RoleExistsAsync(role.RoleName);
            if (exitRole)
            {
                ModelState.AddModelError("role.RoleName", "role not exits");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            //Added the role to identityRole
            await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
            return View(nameof(Index), await _roleManager.Roles.ToListAsync());
        }

        public async Task<IActionResult> MangePermision(string roleid)
        {
            var rule = await _roleManager.FindByIdAsync(roleid);

            if (rule == null)
            {
                return NotFound();
            }

            //get the permistion assigned to rule
            //just we need Value from the Tabel
            var RuleCliam = _roleManager.GetClaimsAsync(rule).Result
            .Select(x => x.Value).ToList();

            //get All permistion founded 
            var AllPermisiton = ModelPermistion.AllPermisitions()
            .Select(x => new CheckViewModel { DisplayValue = x }).ToList();


            //loop in all permistion , and permistion founded in rulecliam mark it as selected value
            foreach (var Permisiton in AllPermisiton)
            {
                if (RuleCliam.Any(x => x == Permisiton.DisplayValue))
                {
                    Permisiton.IsSelected = true;

                }
            }

            var viewModel = new RoleViewModel
            {
                Id = roleid,
                RoleName = rule.Name,
                RuleCliam = AllPermisiton
            };

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> MangePermision(RoleViewModel roleViewModel)
        {
            var rule = await _roleManager.FindByIdAsync(roleViewModel.Id);

            if (rule == null)
            {
                return NotFound();
            }

            //Remove Old permision
            var RuleCliam = await _roleManager.GetClaimsAsync(rule);
            foreach (var rulecliam in RuleCliam)
                await _roleManager.RemoveClaimAsync(rule , rulecliam);

            //new permision
            var permisions = roleViewModel.RuleCliam.Where(x => x.IsSelected).ToList();
            foreach (var permision in permisions)
                await _roleManager.AddClaimAsync(rule, new Claim("Permission", permision.DisplayValue));

            return RedirectToAction(nameof(Index));
        }
    }
}
  
