using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class OneTimePassword
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        public int OTP { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
