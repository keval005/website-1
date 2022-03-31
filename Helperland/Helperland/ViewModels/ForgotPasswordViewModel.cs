using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
