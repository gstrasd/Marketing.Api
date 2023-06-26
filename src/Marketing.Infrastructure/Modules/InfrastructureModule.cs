using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Module = Autofac.Module;

namespace Marketing.Infrastructure.Modules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ => new AsyncLocal<int> { Value = 0 })
                .SingleInstance()
                .Named<AsyncLocal<int>>("SessionId");

            builder.Register(_ => new AsyncLocal<int?> { Value = null })
                .SingleInstance()
                .Named<AsyncLocal<int?>>("IntakeId");

            builder.Register(c => new OrbitDbContext(c.Resolve<Settings>().ConnectionStrings["Orbit"]))
                .InstancePerLifetimeScope()
                .AsSelf();

            builder.Register(c => new PatientRepository(c.Resolve<OrbitDbContext>()))
                .InstancePerLifetimeScope()
                .As<IPatientRepository>();

            builder.Register(c => new IntakeRepository(
                    c.Resolve<OrbitDbContext>(), 
                    c.ResolveNamed<AsyncLocal<int>>("SessionId")))
                .InstancePerLifetimeScope()
                .As<IIntakeRepository>();

            builder.Register(c => new CollectionRepository(
                    c.Resolve<OrbitDbContext>(), 
                    c.Resolve<IIntakeRepository>(),
                    c.Resolve<Settings>(),
                    c.ResolveNamed<AsyncLocal<int>>("SessionId"),
                    c.ResolveNamed<AsyncLocal<int?>>("IntakeId")))
                .InstancePerLifetimeScope()
                .As<ICollectionRepository>();

            builder.Register(c =>
                {
                    using var stream = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Marketing.Infrastructure.Resources.failed-intake-email.html")!);
                    var failedIntake = stream.ReadToEnd();
                    return new NotificationService(c.Resolve<Settings>(), failedIntake);
                })
                .SingleInstance()
                .As<INotificationService>();
        }
    }
}
