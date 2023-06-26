using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain.Entities;

namespace Marketing.Infrastructure.Domain
{
    public interface IPatientRepository
    {
        Task<Patient?> FindPatientAsync(string firstName, string lastName, string phoneNumber);
    }
}
