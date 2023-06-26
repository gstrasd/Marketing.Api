using System.Resources;
using System.Text.Json;
using System.Text.Json.Serialization;
using Marketing.Api.Model.Halda;
using Marketing.Api.Model.Halda.Validators;
using Newtonsoft.Json.Linq;
using Tests.Properties;

namespace Tests
{/*
    public class DeserializationTests
    {
        [Fact]
        public void CanDeserializeTestFormData()
        {
            using var json = new MemoryStream(Resources.test_form_data_submission);
            var lead = JsonSerializer.Deserialize<Submission>(json);
            
            Assert.NotNull(lead);
            Assert.NotNull(lead.SmartForm);
            Assert.NotNull(lead.SmartForm.FormData);
            Assert.NotNull(lead.SmartForm.MetaData);
        }

        [Fact]
        public void CanDeserializeWheelchairOffer()
        {
            using var json = new MemoryStream(Resources.wheelchair_submission);
            var lead = JsonSerializer.Deserialize<Submission>(json);

            Assert.NotNull(lead);
            Assert.NotNull(lead.SmartForm);
            Assert.NotNull(lead.SmartForm.FormData);
            Assert.NotNull(lead.SmartForm.MetaData);

            var validator = new FormDataValidator();
            var result = validator.Validate(lead.SmartForm.FormData);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CannotDeserializeWheelchairOfferFromTestFormDataSubmission()
        {
            using var json = new MemoryStream(Resources.test_form_data_submission);
            var lead = JsonSerializer.Deserialize<Submission>(json);

            Assert.NotNull(lead);
            Assert.NotNull(lead.SmartForm);
            Assert.NotNull(lead.SmartForm.FormData);

            var validator = new FormDataValidator();
            var result = validator.Validate(lead.SmartForm.FormData);

            Assert.False(result.IsValid);
        }
    }*/
}