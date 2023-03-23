using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Permistion.Constant
{
    public static class ModelPermistion
    {
        public static List<string> GenratePrimtionList( string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.delete",
            };
        }


        public static List<string> AllPermisitions()
        {
            var allpermistion = new List<string>();

            var modules = Enum.GetValues(typeof(Module));

            foreach (var module in modules)
            {
                allpermistion.AddRange(GenratePrimtionList(module.ToString()));
            }
            return allpermistion;
        }
    }
}
