using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Marketing.Api.Model.Halda.Validators
{
    public class WheelchairFormDataValidator : AbstractValidator <WheelchairFormData>
    {
        public WheelchairFormDataValidator()
        {
            // Required fields
            RuleFor(formData => formData.FirstName).NotEmpty().WithMessage("First name is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.LastName).NotEmpty().WithMessage("Last name is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Use).NotEmpty().WithMessage("Wheelchair use is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Use).NotEmpty().WithMessage("Wheelchair use is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.PhoneNumber).NotEmpty().WithMessage("Phone number is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Interest).NotEmpty().WithMessage("Interest is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Equipment).NotEmpty().WithMessage("Equipment is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Insurance).NotEmpty().WithMessage("Insurance is required.").WithSeverity(_ => Severity.Error);
            RuleFor(formData => formData.Secondary).NotEmpty().WithMessage("Secondary insurance is required.").WithSeverity(_ => Severity.Error);

            // Optional fields
            RuleFor(formData => formData.EmailAddress).NotEmpty().WithMessage("Email address is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(formData => formData.Contact).NotEmpty().WithMessage("Contact is missing.").WithSeverity(_ => Severity.Warning);
        }
    }
}