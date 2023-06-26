using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Module = Autofac.Module;

namespace Marketing.Application.Modules
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new LeadService(
                    c.Resolve<IPatientRepository>(),
                    c.Resolve<ICollectionRepository>(),
                    c.Resolve<IIntakeRepository>(),
                    c.Resolve<SessionManager>(),
                    c.Resolve<Settings>(), 
                    c.ResolveNamed<AsyncLocal<int?>>("IntakeId"),
                    c.Resolve<ILogger>()))
                .InstancePerLifetimeScope()
                .As<ILeadService>();

            builder.Register(c => new SessionManager(
                    c.Resolve<OrbitDbContext>(),
                    c.ResolveNamed<AsyncLocal<int>>("SessionId")))
                .InstancePerDependency()
                .AsSelf();
        }
    }
}