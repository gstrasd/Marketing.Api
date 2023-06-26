using System;
using System.Text.Json.Serialization;

namespace Marketing.Api.Model.Halda
{
    public class Submission<TFormData> where TFormData : class, new()
    {
        [JsonPropertyName("smartFormSubmission")]
        public SmartForm<TFormData>? SmartForm { get; set; }
    }

    public class SmartForm<TFormData> where TFormData : class, new()
    {
        [JsonPropertyName("id")]
        public string? LeadId { get; set; }
        [JsonPropertyName("createdAt")]
        public string? CreatedAt { get; set; }
        [JsonPropertyName("updatedAt")]
        public string? UpdatedAt { get; set; }
        [JsonPropertyName("submissionDate")]
        public string? SubmissionDate { get; set; }
        [JsonPropertyName("submissionTime")]
        public string? SubmissionTime { get; set; }
        [JsonPropertyName("variantName")]
        public string? VariantName { get; set; }
        [JsonPropertyName("variantId")]
        public string? VariantId { get; set; }
        [JsonPropertyName("firstName")]
        public string? FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
        [JsonPropertyName("formData")]
        public TFormData? FormData { get; set; }
        [JsonPropertyName("metaData")]
        public MetaData? MetaData { get; set; }
    }

    public class MetaData
    {
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }
        [JsonPropertyName("city")]
        public string? City { get; set; }
        [JsonPropertyName("state")]
        public string? State { get; set; }
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        [JsonPropertyName("country_calling_code")]
        public string? CountryCallingCode { get; set; }
        [JsonPropertyName("browser")]
        public string? Browser { get; set; }
        [JsonPropertyName("device_type")]
        public string? DeviceType { get; set; }
        [JsonPropertyName("referring_url")]
        public string? ReferringUrl { get; set; }
    }
}