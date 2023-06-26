using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain.Entities;

namespace Marketing.Infrastructure.Domain
{
    public interface ICollectionRepository
    {
        Task<(Order? Order, Item? Item)> FindLeadAsync(int personId);
        Task<int> CreateLeadAsync(Lead lead, Patient? patient = null);
        Task<int> AddNoteAsync(int collectionId, string subject, string note);
        Task<bool> CheckoutAsync(int collectionId, Flow flow);
        Task CheckInAsync(int collectionId, Flow flow, int outcome = 0);
    }
}