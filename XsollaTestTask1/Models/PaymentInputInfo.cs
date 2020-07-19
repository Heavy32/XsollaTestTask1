using System;
using System.ComponentModel.DataAnnotations;

namespace XsollaTestTask1.Models
{
    public class PaymentInputInfo
    {
        [Required(ErrorMessage = "You must input credit card information")]
        public CreditCard Card { get; set; }
        [Required(ErrorMessage = "You must input credit session ID")]
        public Guid SessionId { get; set; }
        [Required(ErrorMessage = "You must input information about seller")]
        public string Seller { get; set; }
    }
}
