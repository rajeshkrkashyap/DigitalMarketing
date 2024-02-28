using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Shared
{
    public class LoginRegisterMobileViewModel
    {
        public string? CountryCode { get; set; } = "91";

        [Required]
        [StringLength(10)]
        public string? MobileNumber { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string? OTP { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "marketing";

        public string? SessionId { get; set; }
    }
}
