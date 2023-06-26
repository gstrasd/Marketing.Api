using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure
{
    internal class IntakeRepository : IIntakeRepository
    {
        private readonly OrbitDbContext _dbContext;
        private readonly AsyncLocal<int> _sessionId;

        public IntakeRepository(OrbitDbContext dbContext, AsyncLocal<int> sessionId)
        {
            _dbContext = dbContext;
            _sessionId = sessionId;
        }

        public Task<MarketingIntake> GetIntakeRecordAsync(int id) => _dbContext.MarketingIntakes.SingleAsync(i => i.Id == id);

        public async Task<MarketingIntake> RecordIntakeAsync(string leadSource, string payload)
        {
            // Compute payload hash
            var hash = new Guid(MD5.HashData(Encoding.UTF8.GetBytes(payload)));

            var intake = new MarketingIntake
            {
                LeadSource = leadSource,
                Payload = payload,
                PayloadHash = hash,
                CreatedDate = DateTime.Now,
                SessionId = _sessionId.Value
            };

            _dbContext.MarketingIntakes.Add(intake);

            await _dbContext.SaveChangesAsync();
            
            return intake;
        }

        public async Task MarkIntakeSuccessful(int id, int orderId)
        {
            var intake = await _dbContext.MarketingIntakes.SingleAsync(mi => mi.Id == id);

            intake.OrderId = orderId;
            intake.Success = true;
            intake.SessionId = _sessionId.Value;

            await _dbContext.SaveChangesAsync();
        }

        public async Task MarkIntakeFailed(int id, Exception? error)
        {
            var intake = await _dbContext.MarketingIntakes.SingleAsync(mi => mi.Id == id);

            intake.Success = false;
            intake.SessionId = _sessionId.Value;
            intake.Error = error?.ToStringDemystified() ?? null;

            await _dbContext.SaveChangesAsync();
        }
    }
}