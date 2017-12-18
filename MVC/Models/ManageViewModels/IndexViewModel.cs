using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

      
        [Display(Name = "Tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(6, MinimumLength = 6), RegularExpression("[0-9]{4}" + "[A-Z]{2}", ErrorMessage = "Vul een correct postcode in(bijv. 1234AB)")]
        [Display(Name = "Postcode")]
        public string Zipcode { get; set; }

        [Phone]
        [RegularExpression("[0-9]{10}", ErrorMessage = "Vul een 10 cijferife nummer in(bijv. 0612345678)")]
        [Display(Name = "telefoon nummer")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
