using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ManageViewModels
{
    public class EnableAuthenticatorViewModel
    {
            [Required]
            [StringLength(7, ErrorMessage = "De {0} moet min {2} en max {1} karakters lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verificatie Code")]
            public string Code { get; set; }

            [ReadOnly(true)]
            public string SharedKey { get; set; }

            public string AuthenticatorUri { get; set; }
    }
}
