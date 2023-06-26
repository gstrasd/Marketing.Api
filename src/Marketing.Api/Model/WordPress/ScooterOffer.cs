using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Marketing.Api.Model.WordPress
{
    public class ScooterOffer
    {
        [JsonPropertyName("_date")]
        public string Date { get; set; } = null!;
        [JsonPropertyName("_time")]
        public string Time { get; set; } = null!;
        [JsonPropertyName("_serial_number")]
        public string? SerialNumber { get; set; }
        [JsonPropertyName("_post_name")]
        public string PostName { get; set; } = null!;
        [JsonPropertyName("your-name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;
        [JsonPropertyName("phone")]
        public string Phone { get; set; } = null!;
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("doyouhaveinsurance")]
        public string? HasInsurance { get; set; }
        [JsonPropertyName("insurance-type")]
        public string? InsuranceType { get; set; }
        [JsonPropertyName("version")]
        public string? InsuranceVersion { get; set; }
        [JsonPropertyName("other")]
        public string? OtherInsurance { get; set; }
        [JsonPropertyName("offer")]
        public string? OfferNumber { get; set; }
    }
}