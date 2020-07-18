using System;
using System.ComponentModel.DataAnnotations;

namespace XsollaTestTask1.Models
{
    public class PaymentInputInfo
    {
        [Required]
        public CreditCard card { get; set; }
        [Required]
        public Guid SessionId { get; set; }
        [Required]
        public string Seller { get; set; }
    }
}
