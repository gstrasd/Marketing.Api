using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain.Entities;

namespace Marketing.Infrastructure.Domain
{
    public interface INotificationService
    {
        Task SendFailedIntakeEmailAsync(MarketingIntake intake, string payload, Exception? error = null);
    }
}