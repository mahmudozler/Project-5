﻿using System;
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
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }


        [Phone]
        [Display(Name = "telefoon nummer")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
