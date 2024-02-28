using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class CheckoutOrderResponse
    {
        public string? SessionId { get; set; }
        public string? PubKey { get; set; }
    }
}
