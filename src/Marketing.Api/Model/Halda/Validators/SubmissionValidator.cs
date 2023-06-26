using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Marketing.Api.Model.Halda.Validators
{
    public class SubmissionValidator<TFormData> : AbstractValidator<Submission<TFormData>> where TFormData : class, new()
    {
        public SubmissionValidator(SmartFormValidator<TFormData> validator)
        {
            RuleFor(submission => submission.SmartForm).NotNull().WithMessage("Smart form is required.").WithSeverity(_ => Severity.Error).SetValidator(validator!);
        }
    }

    public class SmartFormValidator<TFormData> : AbstractValidator<SmartForm<TFormData>> where TFormData : class, new()
    {
        public SmartFormValidator(IValidator<TFormData> validator)
        {
            // Required fields
            RuleFor(form => form.LeadId).NotEmpty().WithMessage("Lead id is required.").WithSeverity(_ => Severity.Error);
            RuleFor(form => form.VariantId).NotEmpty().WithMessage("Variant id is required.").WithSeverity(_ => Severity.Error);
            RuleFor(form => form.FormData).NotNull().WithMessage("Form data is required.").WithSeverity(_ => Severity.Error).SetValidator(validator!);

            // Optional fields
            RuleFor(form => form.SubmissionDate).NotEmpty().WithMessage("Submission date is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.SubmissionTime).NotEmpty().WithMessage("Submission time is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.FirstName).NotEmpty().WithMessage("First name is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.LastName).NotEmpty().WithMessage("Last name is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.Email).NotEmpty().WithMessage("Email is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.CreatedAt).NotEmpty().WithMessage("Created at date is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.UpdatedAt).NotEmpty().WithMessage("Updated at date is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.VariantName).NotEmpty().WithMessage("Variant name is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.PhoneNumber).NotEmpty().WithMessage("Phone number is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(form => form.MetaData).NotNull().WithMessage("Metadata is missing.").WithSeverity(_ => Severity.Warning).SetValidator(new MetaDataValidator()!);
        }
    }

    public class MetaDataValidator : AbstractValidator<MetaData>
    {
        public MetaDataValidator()
        {
            RuleFor(metaData => metaData.IpAddress).NotEmpty().WithMessage("Ip address is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.City).NotEmpty().WithMessage("City is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.State).NotEmpty().WithMessage("State is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.Country).NotEmpty().WithMessage("Country is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.CountryCallingCode).NotEmpty().WithMessage("Country calling code is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.Browser).NotEmpty().WithMessage("Browser is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.DeviceType).NotEmpty().WithMessage("Device type is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(metaData => metaData.ReferringUrl).NotEmpty().WithMessage("Referring url is missing.").WithSeverity(_ => Severity.Warning);
        }
    }
}