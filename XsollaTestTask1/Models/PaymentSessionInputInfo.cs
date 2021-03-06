﻿using System;
using System.ComponentModel.DataAnnotations;

namespace XsollaTestTask1.Models
{
    public class PaymentSessionInputInfo
    {
        [Required(ErrorMessage = "You must enter the sum")]
        [Range(0, Double.MaxValue, ErrorMessage = "The sum must be more than 0")]
        public double Sum { get; set; }
        [Required(ErrorMessage = "You must enter the payment appointemnt")]
        public string PaymentAppointement { get; set; }
    }
}
