using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XsollaTestTask1.Models
{
    public class PaymentInputInfo
    {
        [Required]
        public CreditCard card { get; set; }
        [Required]
        public Guid SessionId { get; set; }
    }
}
