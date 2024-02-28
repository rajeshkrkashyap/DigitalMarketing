using Microsoft.AspNetCore.Identity;
using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.AccountRecharge
{
    public class DigitalWallet
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CreditMoney { get; set; }
        public string CreditNotes { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DebitMoney { get; set; }
        public string? DebitNotes { get; set; }
        public string? Currency { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? Signature { get; set; }
        public string? PaymentGatewayResponse { get; set; }
        public string? GatewayName { get; set; }
        public string? CreatedBy { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Updated { get; set; }
        public bool IsActive { get; set; }
        public AppUser AppUser { get; set; }
    }
}
