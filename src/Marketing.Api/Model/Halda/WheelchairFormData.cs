using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Marketing.Api.Model.Halda
{
    public class WheelchairFormData
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = null!;
        [JsonPropertyName("email_address")]
        public string? EmailAddress { get; set; }
        [JsonPropertyName("use_answer")]
        public string? Use { get; set; }
        [JsonPropertyName("contact_answer")]
        public string? Contact { get; set; }
        [JsonPropertyName("interest_answer")]
        public string Interest { get; set; } = null!;
        [JsonPropertyName("equipment_answer")]
        public string? Equipment { get; set; }
        [JsonPropertyName("insurance_answer")]
        public string? Insurance { get; set; }
        [JsonPropertyName("secondary_answer")]
        public string? Secondary { get; set; }
    }
}