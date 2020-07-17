using XsollaTestTask1.Contexts;
using Microsoft.EntityFrameworkCore;

namespace XsollaTestTask1Tests.ControllerTests
{
    public static class ContextBuilder
    {
        public static PaymentSessionDBContext BuildPaymentSessionContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<PaymentSessionDBContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new PaymentSessionDBContext(options);
            return dbContext;
        }

        public static ReceiptDBContext BuildReceiptContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ReceiptDBContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new ReceiptDBContext(options);
            return dbContext;
        }
    }
}
