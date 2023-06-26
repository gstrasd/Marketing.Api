using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Marketing.Infrastructure
{
    internal class PatientRepository : IPatientRepository
    {
        private readonly DbContext _dbContext;

        public PatientRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient?> FindPatientAsync(string firstName, string lastName, string phoneNumber)
        {
            if (String.IsNullOrWhiteSpace(firstName) || String.IsNullOrWhiteSpace(lastName)) return null;

            var scrubbedFirstName = firstName.Replace("'", "").Trim();
            var scrubbedLastName = lastName.Replace("'", "").Trim();
            var scrubbedPhone = Regex.Replace(phoneNumber, @"[^\d]*", "");
            if (scrubbedPhone.Length != 10) return null;

            var patients = await _dbContext.Database.SqlQueryRaw<Patient>($"exec dbo.spfindpatients @find='{scrubbedPhone}', @dsout=1").ToListAsync();
            var patient = patients
                .OrderByDescending(p => p.PersonId)
                .FirstOrDefault(p =>
                    p.FirstName!.Trim().Equals(scrubbedFirstName, StringComparison.OrdinalIgnoreCase)
                    && p.LastName!.Trim().Equals(scrubbedLastName, StringComparison.OrdinalIgnoreCase)
                );

           return patient;
        }
    }
}