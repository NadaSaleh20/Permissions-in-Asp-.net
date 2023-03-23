using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permistion.Filters
{
    public class PermisonsRequrments : IAuthorizationRequirement
    {
        public string Permistions { get; private set; }
        public PermisonsRequrments(string permision)
        {
            Permistions = permision;
        }
    }
}
