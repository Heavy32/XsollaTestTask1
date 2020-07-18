using System;

namespace XsollaTestTask1.Models
{
    public class Receipt
    {
        public Guid Id { get; set; }
        public DateTime OperationTime { get; set; }
        public string Seller { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public double Cost { get; set; }
    }
}
