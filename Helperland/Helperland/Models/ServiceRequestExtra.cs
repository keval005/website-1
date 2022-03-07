using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Helperland.Models
{
    public class ServiceRequestExtra
    {
        [Key]
        public int ServiceRequestExtraId { get; set; }
        public int ServicesRequestId { get; set; }
        public int ServicesExtraId { get; set; }
    }
}
