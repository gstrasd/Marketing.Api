using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using FluentValidation;
using Marketing.Api.Factories;
using Marketing.Api.Model.Halda;
using Marketing.Api.Model.WordPress;
using Marketing.Application.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marketing.Api.Controllers
{
    [Route("marketing/[controller]")]
    [ApiController]
    public class WordPressController : ControllerBase
    {
        private readonly IValidator<PowerChairOffer> _powerChairValidator;
        private readonly IValidator<ScooterOffer> _scooterOfferValidator;
        private readonly ILeadService _leadService;
        private readonly DomainFactory _factory;

        public WordPressController(IValidator<PowerChairOffer> powerChairValidator, IValidator<ScooterOffer> scooterOfferValidator, ILeadService leadService, DomainFactory factory)
        {
            _powerChairValidator = powerChairValidator;
            _scooterOfferValidator = scooterOfferValidator;
            _leadService = leadService;
            _factory = factory;
        }

        [HttpPost]
        [Route("power-wheelchair-offer")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task ContactAsync(PowerChairOffer offer)
        {
            var result = await _powerChairValidator.ValidateAsync(offer);
            if (result.Errors.Any(e => e.Severity == Severity.Error)) throw new ValidationException(result.Errors);

            var lead = _factory.CreateLead(offer);
            await _leadService.SubmitLeadAsync(lead);
        }

        [HttpPost]
        [Route("scooter-offer-2")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task ContactAsync(ScooterOffer offer)
        {
            var result = await _scooterOfferValidator.ValidateAsync(offer);
            if (result.Errors.Any(e => e.Severity == Severity.Error)) throw new ValidationException(result.Errors);
            
            var lead = _factory.CreateLead(offer);
            await _leadService.SubmitLeadAsync(lead);
        }
    }
}