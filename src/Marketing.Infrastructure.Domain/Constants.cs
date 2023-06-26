using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketing.Infrastructure.Domain
{
    public enum Domain : byte
    {
        None = 0,
        Marketing = 48
    }

    public enum Status : short
    {
        None = 0,
        Ready,
        Active,
        Hold,
        Routed,
        Waiting,
        Completed,
        SystemWait,
        UserWait,
        WaitingOnAction,
        Unworkable,
        NonBillable
    }

    public enum Tasks
    {
        None = 0,
        InsideLeads = 87,
        MarketingIntake = 89
    }

    public enum Flow : short
    {
        None = 0,
        InsideLeads = 1200,
        BusinessIntelligence = 1201,
        OrderComplete = 1203
    }
}
