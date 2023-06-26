using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Marketing.Api.Controllers;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Identity.Client;

namespace Marketing.Api.Middleware
{
    public class IntakeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IIntakeRepository _intakeRepository;
        private readonly INotificationService _notificationService;
        private readonly AsyncLocal<int?> _intakeId;

        public IntakeMiddleware(RequestDelegate next, IIntakeRepository intakeRepository, INotificationService notificationService, AsyncLocal<int?> intakeId)
        {
            _next = next;
            _intakeRepository = intakeRepository;
            _notificationService = notificationService;
            _intakeId = intakeId;
        }

        public async Task Invoke(HttpContext context)
        {
            // This middleware logic should only execute when receiving leads from Halda or GoogleAds.
            // This logic exists here to capture the request payload early in the pipeline before an error could occur; otherwise, a lead could be missed.
            var controller = context.Request.RouteValues["controller"] as string;
            var action = context.Request.RouteValues["action"] as string;
            if ((controller == "Halda" && action == "SubmitLead") || (controller == "WordPress" && action == "Contact"))
            {
                var leadSource = controller == "Halda" ? "Halda" : controller == "WordPress" ? "GoogleAds" : "";
                var request = context.Request;
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                request.EnableBuffering();
                
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                var payload = Encoding.UTF8.GetString(buffer);
                request.Body.Seek(0, SeekOrigin.Begin);
                
                var intake = await _intakeRepository.RecordIntakeAsync(leadSource, payload);
                _intakeId.Value = intake.Id;

                try
                {
                    await _next.Invoke(context);

                    // Check if intake was marked successful
                    intake = await _intakeRepository.GetIntakeRecordAsync(intake.Id);
                    if (intake is { Success: false }) await _notificationService.SendFailedIntakeEmailAsync(intake, payload);
                }
                catch (Exception e)
                {
                    await _intakeRepository.MarkIntakeFailed(_intakeId.Value!.Value, e);
                    await _notificationService.SendFailedIntakeEmailAsync(intake, payload);
                    
                    throw e;
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
