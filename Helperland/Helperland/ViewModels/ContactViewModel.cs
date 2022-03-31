using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a mobile number")]
        [RegularExpression(@"[0-9]{10}$", ErrorMessage = "Enter 10 digit valid phone number.")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Please enter valid Email.")]
        public string EmailAddress { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter a message")]
        public string Message { get; set; }

        public IFormFile Attachment { get; set; }
    }
}
