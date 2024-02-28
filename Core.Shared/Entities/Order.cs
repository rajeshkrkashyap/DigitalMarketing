using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Entities
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? CustomerName { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public double? TotalAmount { get; set; }
        public string? Countery { get; set; }
        public string? City { get; set; }
    }
}
