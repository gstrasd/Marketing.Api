using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marketing.Infrastructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Marketing.Infrastructure.Domain
{
    public class OrbitDbContext : SqlServerDbContext
    {
        public OrbitDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;
        public DbSet<MarketingIntake> MarketingIntakes { get; set; } = null!;
    }
}