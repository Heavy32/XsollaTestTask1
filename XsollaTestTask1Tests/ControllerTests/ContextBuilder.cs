using XsollaTestTask1.Contexts;
using Microsoft.EntityFrameworkCore;

namespace XsollaTestTask1Tests.ControllerTests
{
    public static class ContextBuilder
    {
        public static PaymentDbContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<PaymentDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new PaymentDbContext(options);
            return dbContext;
        }
    }
}
