﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Helperland.ViewModels
{
    public class ServiceRequestViewModel
    {
        [JsonPropertyName("serviceRequestId")]
        public int ServiceRequestId { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("postalCode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("serviceStartDate")]
        public string ServiceStartDate { get; set; }

        [JsonPropertyName("serviceStartTime")]
        public string ServiceStartTime { get; set; }

        [JsonPropertyName("serviceHourlyRate")]
        public int ServiceHourlyRate { get; set; }

        [JsonPropertyName("serviceHours")]
        public float ServiceHours { get; set; }

        [JsonPropertyName("extraHours")]
        public float ExtraHours { get; set; }

        [JsonPropertyName("extraServicesName")]
        public string[] ExtraServicesName { get; set; }

        [JsonPropertyName("subTotal")]
        public float SubTotal { get; set; }

        [JsonPropertyName("totalCost")]
        public float TotalCost { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("hasPets")]
        public bool HasPets { get; set; }

        [JsonPropertyName("refundedAmount")]
        public decimal RefundedAmount { get; set; }

        [JsonPropertyName("userAddressId")]
        public string UserAddressId { get; set; }

        [JsonPropertyName("paymentDone")]
        public bool PaymentDone { get; set; }

        [JsonPropertyName("recordVersion")]
        public string RecordVersion { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }
}
