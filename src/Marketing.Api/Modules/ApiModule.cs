using System.Threading;
using Autofac;
using FluentValidation;
using Marketing.Api.Controllers;
using Marketing.Api.Extensions;
using Marketing.Api.Factories;
using Marketing.Api.Model.Halda;
using Marketing.Api.Model.Halda.Validators;
using Marketing.Api.Model.WordPress;
using Marketing.Api.Model.WordPress.Validators;
using Marketing.Application;
using Marketing.Application.Domain;
using Marketing.Application.Modules;
using Marketing.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.File;

namespace Marketing.Api.Modules
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => c.Resolve<IConfiguration>().Bind<Settings>()).SingleInstance().AsSelf();

            builder.Register(_ => new SubmissionValidator<WheelchairFormData>(new SmartFormValidator<WheelchairFormData>(new WheelchairFormDataValidator())))
                .SingleInstance()
                .As<IValidator<Submission<WheelchairFormData>>>();

            builder.Register(_ => new PowerChairOfferValidator()).SingleInstance().As<IValidator<PowerChairOffer>>();

            builder.Register(_ => new ScooterOfferValidator()).SingleInstance().As<IValidator<ScooterOffer>>();

            builder.Register(_ => new DomainFactory()).SingleInstance().AsSelf();

            builder.Register(c => new LoggerConfiguration().ReadFrom.Configuration(c.Resolve<IConfiguration>()).CreateLogger()).SingleInstance().As<ILogger>();
        }
    }
}