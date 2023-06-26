using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain.Entities;

namespace Marketing.Infrastructure.Domain
{
    public interface IIntakeRepository
    {
        Task<MarketingIntake> GetIntakeRecordAsync(int id);
        Task<MarketingIntake> RecordIntakeAsync(string leadSource, string payload);
        Task MarkIntakeSuccessful(int id, int orderId);
        Task MarkIntakeFailed(int id, Exception? error);
    }
}