using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Marketing.Infrastructure.Domain.Entities
{
    [PrimaryKey(nameof(PersonId))]
    public class Patient
    {
        public int PersonId { get; set; }
        [Column("Patient")]
        public string? PatientName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Column("DOB")]
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? HomePhone { get; set; }
        public string? CellPhone { get; set; }
        public string? OfficePhone { get; set; }
        public string? BestPhone { get; set; }
    }
}
