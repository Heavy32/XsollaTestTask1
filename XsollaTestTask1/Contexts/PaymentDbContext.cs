using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.Contexts
{
    public class PaymentDbContext : IdentityDbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PaymentSession> PaymentSessions { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
    }
}
