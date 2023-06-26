using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Marketing.Api.Model.WordPress.Validators
{
    public class ScooterOfferValidator : AbstractValidator<ScooterOffer>
    {
        public ScooterOfferValidator()
        {
            // Required fields
            RuleFor(offer => offer.Name).NotEmpty().WithMessage("Name is required.").WithSeverity(_ => Severity.Error);
            RuleFor(offer => offer.Email).NotEmpty().WithMessage("Email is required.").WithSeverity(_ => Severity.Error);
            RuleFor(offer => offer.Phone).NotEmpty().WithMessage("Phone is required.").WithSeverity(_ => Severity.Error);

            // Optional fields
            RuleFor(offer => offer.State).NotEmpty().WithMessage("State is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(offer => offer.OfferNumber).NotEmpty().WithMessage("Offer number is required.").WithSeverity(_ => Severity.Warning);
            RuleFor(offer => offer.HasInsurance).NotEmpty().WithMessage("Has insurance is required.").WithSeverity(_ => Severity.Warning);
            RuleFor(offer => offer.InsuranceType).NotEmpty().WithMessage("Insurance type is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(offer => offer.InsuranceVersion).NotEmpty().WithMessage("Insurance version is missing.").WithSeverity(_ => Severity.Warning);
            RuleFor(offer => offer.OtherInsurance).NotEmpty().WithMessage("Other insurance is missing.").WithSeverity(_ => Severity.Warning);
        }
    }
}