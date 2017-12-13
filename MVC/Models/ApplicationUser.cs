using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MVC.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }

        [RegularExpression("[0-9]{4}"+"[A-Z]{2}",ErrorMessage = "Vul een correct postcode in(bijv. 1234AB)")]
        public string Zipcode { get; set; }

        [RegularExpression("[0-9]{10}",ErrorMessage = "Vul een 10 cijferife nummer in(bijv. 0612345678)")]
        public override string PhoneNumber { get; set; }


    }
}
