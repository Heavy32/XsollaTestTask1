using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XsollaTestTask1.Models
{
    public class PaymentSession
    {
        [Key]
        public Guid SessionId { get; set; }
        public double Cost { get; set; }
        public string PaymentAppointment {get; set;}
        public DateTime SessionRegistrationTime { get; set; }
        public int LifeSpanInMinute { get; set; }
        public string Seller { get; set; }
    }
}
