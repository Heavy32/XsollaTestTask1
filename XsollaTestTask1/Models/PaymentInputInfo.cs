using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsollaTestTask1.Models
{
    public class PaymentInputInfo
    {
        public CreditCard card { get; set; }
        public Guid SessionId { get; set; }
    }
}
