using System;

namespace Marketing.Api.Security
{
    public class GlobalBearerToken
    {
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
