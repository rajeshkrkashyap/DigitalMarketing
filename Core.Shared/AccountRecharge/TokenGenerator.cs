using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.AccountRecharge
{
    public class TokenGenerator
    {
        private decimal conversionRate = 0.1m; // Conversion rate: 1 unit of currency = 0.1 token
        public int GenerateTokens(decimal rechargeAmount)
        {
            // Generate tokens based on the recharge amount
            int tokens = (int)(rechargeAmount * conversionRate);

            // Alternatively, you can use a fixed conversion rate
            // int tokens = (int)(rechargeAmount / conversionRate);

            return tokens;
        }
    }

}
