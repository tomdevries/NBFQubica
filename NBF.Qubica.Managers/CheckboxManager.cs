using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBF.Qubica.CMS
{
    public static class CheckboxManager
    {
        /// <summary>
        /// for get CheckboxBowlingcenter for specific id
        /// </summary>
        public static C_Checkbox Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// for get all CheckboxBowlingcenter
        /// </summary>
        public static IEnumerable<C_Checkbox> GetAll()
        {
            List<C_Checkbox> cbbcl = new List<C_Checkbox>();
            List<S_BowlingCenter> bcl = BowlingCenterManager.GetBowlingCenters();

            foreach (S_BowlingCenter bc in bcl)
                cbbcl.Add(new C_Checkbox { Name = bc.name, Id = bc.id });

            return cbbcl;
        }
    }
}