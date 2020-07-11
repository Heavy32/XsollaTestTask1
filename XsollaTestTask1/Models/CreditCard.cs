using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XsollaTestTask1.Models
{
    public class CreditCard
    {
        public long Number { get; set; }
        public string HolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int CVV { get; set; }
    }
}
