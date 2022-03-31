using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,16}$", ErrorMessage = "You must enter At least one upper case, one lower case, one digit and Minimum six and Maximum 16 in length")]
        public String NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Password and Confirm Password do not match")]
        public String NewConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
