using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.Contexts
{
    public class PaymentSessionDBContext : DbContext
    {
        public PaymentSessionDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PaymentSession> PaymentSessions { get; set; }
    }
}
