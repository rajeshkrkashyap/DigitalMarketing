using Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared
{
    public class UserDetail
    {
        public string? Role { get; set; }
        public string? Name { get; set; }
        public string? UserID { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Email { get; set; }
        public string? UserAvatar { get; set; }
        public decimal Tokens { get; set; }
        public string? Language { get; set; }
        public string? CountryCode { get; set; }
        public string? Gender { get; set; }

    }
}
