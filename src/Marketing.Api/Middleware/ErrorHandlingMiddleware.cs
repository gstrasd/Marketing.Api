using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Marketing.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException e)
            {
                var errors = e.Errors.Where(e => e.Severity == Severity.Error).ToList();
                var warnings = e.Errors.Where(e => e.Severity == Severity.Warning).ToList();
                var details = new ProblemDetails
                {
                    Title = "An error occurred during input validation.",
                    Status = StatusCodes.Status400BadRequest,
                };

                details.Extensions.Add("errors", errors.Select(e => e.ErrorMessage));
                if (warnings.Any()) details.Extensions.Add("warnings", warnings.Select(w => w.ErrorMessage));

                var exception = new ProblemDetailsException(details);

                _logger.Error(e, "An error occurred during input validation.");

                throw exception;
            }
            catch (Exception e)
            {
                var details = new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = StatusCodes.Status500InternalServerError,
                };
                var exception = new ProblemDetailsException(details, e);

                _logger.Error(e, "An unexpected error occurred.");

                throw exception;
            }
        }
    }
}
