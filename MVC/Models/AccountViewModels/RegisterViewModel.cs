using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Tussenvoegsel")]
        public string Tussenvoegsel { get; set; }

        [Required]
        [Display(Name = "Achternaam")]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6), RegularExpression("[0-9]{4}" + "[A-Z]{2}", ErrorMessage = "Vul een correct postcode in(bijv. 1234AB)")]
        [Display(Name = "Postcode")]
        public string Zipcode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "De {0} moet min {2} en max {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De nieuwe wachtwoord en bevestigde wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }

    }
}
