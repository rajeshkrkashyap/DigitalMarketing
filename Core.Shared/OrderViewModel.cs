using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text;

namespace Core.Shared
{
    public class OrderViewModel
    {
        public string OrderId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string? ServiceId { get; set; }
        public string? Tokens { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal? BaseCost{ get; set; }
        public string? SubscriptionType { get; set; }
        public string? Mobile { get; set; }
        public string? CountryCode { get; set; }
        public string Amount { get; set; }
  
    }
}
