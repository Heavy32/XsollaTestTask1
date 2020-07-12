using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.Contexts
{
    public class ReceiptDBContext : DbContext
    {
        public ReceiptDBContext(DbContextOptions<ReceiptDBContext> options) : base(options)
        {
        }

        public Receipt Receipts { get; set; }
    }
}
