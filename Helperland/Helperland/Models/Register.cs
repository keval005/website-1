using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Models
{
    public class Register
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public String Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public Double PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password Is Incorrect")]
        public String PassWord { get; set; }
        [Required]
        [Compare("PassWord", ErrorMessage = "Password And Confirm Password Is Not Same")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
