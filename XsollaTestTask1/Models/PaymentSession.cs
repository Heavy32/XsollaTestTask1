using System;
using System.ComponentModel.DataAnnotations;

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
    }
}
