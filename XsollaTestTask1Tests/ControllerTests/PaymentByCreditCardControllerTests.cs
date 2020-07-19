using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Linq;
using XsollaTestTask1.Controllers;
using XsollaTestTask1.Models;
using Microsoft.AspNetCore.Mvc;

namespace XsollaTestTask1Tests.ControllerTests
{
    class PaymentByCreditCardControllerTests
    {
        [Test]
        public async Task Get_All_Payment_History_For_1_Day_Return_3_Receipts()
        {
            //Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(databaseName);
            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-5)
            });

            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-15)
            });

            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-10)
            });

            await context.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(context);

            //Act 
            var response = await controller.GetAllPaymentHistory(1);
            var count = response.Value.Count;

            //Assert
            Assert.AreEqual(3, count);
        }

        [Test]
        public async Task Get_All_Payment_History_For_1_Day_Return_0_Receipts()
        {
            //Arrange
            var databaseName1 = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(databaseName1);
            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-5)
            });

            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-15)
            });

            context.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-10)
            });

            await context.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(context);

            //Act 
            var response = await controller.GetAllPaymentHistory(1);
            var count = response.Value.Count;

            //Assert
            Assert.AreEqual(0, count);
        }

        [Test]
        public async Task Successful_Payment()
        {
            //Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(databaseName);

            context.PaymentSessions.Add(new PaymentSession
            {
                Cost = 200,
                PaymentAppointment = "Item1",
                LifeSpanInMinute = 60,
                SessionRegistrationTime = DateTime.Now.AddMinutes(-30),
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e")
            });

            await context.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(context);

            CreditCard card = new CreditCard
            {
                Number = "1111 2222 3333 4444",
                CVV = "123",
                HolderName = "Alex",
                ExpirationDate = new DateTime(2021, 10, 1)
            };

            var paymentInfo = new PaymentInputInfo 
            { 
                card = card, 
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e"),
                Seller = "www.google.com"
            };


            //Act
            await controller.PayByCreditCard(paymentInfo);
            var count = context.Receipts.Count();


            //Assert
            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task Unsuccessful_Payment_Session_Is_Over()
        {
            //Arrange
            var DatabaseName = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(DatabaseName);

            context.PaymentSessions.Add(new PaymentSession
            {
                Cost = 200,
                PaymentAppointment = "Item1",
                LifeSpanInMinute = 60,
                SessionRegistrationTime = DateTime.Now.AddMinutes(-90),
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e")
            });

            await context.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(context);

            CreditCard card = new CreditCard
            {
                Number = "1111 2222 3333 4444",
                CVV = "123",
                HolderName = "Alex",
                ExpirationDate = new DateTime(2021, 10, 1)
            };

            var paymentInfo = new PaymentInputInfo
            {
                card = card,
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e"),
            };


            //Act
            var response = await controller.PayByCreditCard(paymentInfo);
            var count = context.Receipts.Count();


            //Assert
            Assert.AreEqual(0, count);
            Assert.NotNull(response is BadRequestObjectResult);
        }
    }
}
