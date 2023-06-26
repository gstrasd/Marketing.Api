//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Json;
//using System.Threading.Tasks;
//using FluentValidation;
//using Marketing.Api.Model.Halda;
//using Marketing.Api.Model.Halda.Validators;
//using Tests.Properties;

//namespace Tests
//{
//    public class ValidationTests
//    {
//        [Fact]
//        public void CanValidateWheelchairOffer()
//        {
//            using var json = new MemoryStream(Resources.wheelchair_submission);
//            var lead = JsonSerializer.Deserialize<Submission>(json);
//            var offer = lead?.SmartForm?.WheelchairFormData;
//            var validator = new FormDataValidator();
//            var result = validator.Validate(offer!);

//            Assert.True(result.IsValid);
//        }

//        [Fact]
//        public void CannotValidateInvalidWheelchairOffer()
//        {
//            var offer = new WheelchairOffer();
//            var validator = new FormDataValidator();
//            var result = validator.Validate(offer);

//            Assert.False(result.IsValid);
//            Assert.Equal(0, result.Errors.Count(e => e.Severity == Severity.Warning));
//            Assert.Equal(6, result.Errors.Count(e => e.Severity == Severity.Error));
//            Assert.Equal("No equipment preference provided.", result.Errors[3].ErrorMessage);
//        }

//        [Fact]
//        public void CanValidateMetaData()
//        {
//            using var json = new MemoryStream(Resources.wheelchair_submission);
//            var lead = JsonSerializer.Deserialize<Submission>(json);
//            var metadata = lead?.SmartForm?.MetaData;
//            var validator = new MetaDataValidator();
//            var result = validator.Validate(metadata!);

//            Assert.True(result.IsValid);
//        }

//        [Fact]
//        public void CannotValidateInvalidMetadata()
//        {
//            var metadata = new MetaData();
//            var validator = new MetaDataValidator();
//            var result = validator.Validate(metadata);

//            Assert.False(result.IsValid);
//            Assert.Equal(8, result.Errors.Count(e => e.Severity == Severity.Warning));
//            Assert.Equal(0, result.Errors.Count(e => e.Severity == Severity.Error));
//        }

//        [Fact]
//        public void CanValidateFullWheelchairOfferLead()
//        {
//            using var json = new MemoryStream(Resources.wheelchair_submission);
//            var lead = JsonSerializer.Deserialize<Submission>(json);
//            var validator = new SubmissionValidator<WheelchairOffer>(new FormDataValidator());
//            var result = validator.Validate(lead!);

//            Assert.NotNull(lead);
//            Assert.True(result.IsValid);
//        }

//        [Fact]
//        public void CannotValidateInvalidFullWheelchairOfferLead()
//        {
//            var lead = new Submission();
//            var validator = new SubmissionValidator<WheelchairOffer>(new FormDataValidator());
//            var result = validator.Validate(lead!);

//            Assert.False(result.IsValid);
//            Assert.Equal(0, result.Errors.Count(e => e.Severity == Severity.Warning));
//            Assert.Equal(1, result.Errors.Count(e => e.Severity == Severity.Error));
//        }
//    }
//}