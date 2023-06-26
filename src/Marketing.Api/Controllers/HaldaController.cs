using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Marketing.Application.Domain;
using Marketing.Application.Domain.Halda;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Marketing.Api.Factories;
using Marketing.Api.Model.Halda;
using Marketing.Application;
using Marketing.Infrastructure.Domain;

namespace Marketing.Api.Controllers
{
    [Route("marketing/[controller]")]
    [ApiController]
    public class HaldaController : ControllerBase
    {
        private readonly IValidator<Submission<WheelchairFormData>> _wheelchairSubmissionValidator;
        private readonly ILeadService _leadService;
        private readonly DomainFactory _factory;

        public HaldaController(IValidator<Submission<WheelchairFormData>> wheelchairSubmissionValidator, ILeadService leadService, DomainFactory factory)
        {
            _wheelchairSubmissionValidator = wheelchairSubmissionValidator;
            _leadService = leadService;
            _factory = factory;
        }

        [HttpPost]
        [Route("wheelchair")]
        [Authorize]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task SubmitLeadAsync([FromBody] Submission<WheelchairFormData> submission)
        {
            var result = await _wheelchairSubmissionValidator.ValidateAsync(submission);
            if (result.Errors.Any(e => e.Severity == Severity.Error)) throw new ValidationException(result.Errors);
            
            var lead = _factory.CreateLead(submission.SmartForm!);
            await _leadService.SubmitLeadAsync(lead);
        }
    }
}