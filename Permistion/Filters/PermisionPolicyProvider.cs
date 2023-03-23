using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permistion.Filters
{
    public class PermisionPolicyProvider : IAuthorizationPolicyProvider
    {

        public DefaultAuthorizationPolicyProvider FallBackPolicyProvider { get; }

        public PermisionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallBackPolicyProvider=  new DefaultAuthorizationPolicyProvider(options);
        }
        Task<AuthorizationPolicy> IAuthorizationPolicyProvider.GetDefaultPolicyAsync()
        {
            return FallBackPolicyProvider.GetDefaultPolicyAsync();
        }

        Task<AuthorizationPolicy> IAuthorizationPolicyProvider.GetFallbackPolicyAsync()
        {
           return FallBackPolicyProvider.GetDefaultPolicyAsync();
        }

        Task<AuthorizationPolicy> IAuthorizationPolicyProvider.GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permission"))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermisonsRequrments(policyName));
                return Task.FromResult(policy.Build());
            }
            return FallBackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
