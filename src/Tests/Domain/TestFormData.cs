using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tests.Domain
{
    public class TestFormData
    {
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }
        [JsonPropertyName("funnel_stage")]
        public string? FunnelStage { get; set; }
        [JsonPropertyName("phone_number")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("email_address")]
        public string? EmailAddress { get; set; }
        [JsonPropertyName("biggest_concern")]
        public string? BiggestConcern { get; set; }
    }
}