using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Web.Http.Results;
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
            var databaseName1 = Guid.NewGuid().ToString();
            var context1 = ContextBuilder.BuildReceiptContext(databaseName1);
            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-5)
            });

            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-15)
            });

            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddHours(-10)
            });

            await context1.SaveChangesAsync();

            var databaseName2 = Guid.NewGuid().ToString();
            var context2 = ContextBuilder.BuildPaymentSessionContext(databaseName2);

            var controller = new PaymentByCreditCardController(context1, context2);

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
            var context1 = ContextBuilder.BuildReceiptContext(databaseName1);
            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-5)
            });

            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-15)
            });

            context1.Receipts.Add(new Receipt
            {
                Id = Guid.NewGuid(),
                Cost = 200,
                CustomerName = "Alex",
                Product = "Chips",
                OperationTime = DateTime.Now.AddDays(-10)
            });

            await context1.SaveChangesAsync();

            var databaseName2 = Guid.NewGuid().ToString();
            var context2 = ContextBuilder.BuildPaymentSessionContext(databaseName2);

            var controller = new PaymentByCreditCardController(context1, context2);

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
            var sessionDatabaseName = Guid.NewGuid().ToString();
            var sessionContext = ContextBuilder.BuildPaymentSessionContext(sessionDatabaseName);

            var receiptDatabaseName = Guid.NewGuid().ToString();
            var receiptContext = ContextBuilder.BuildReceiptContext(receiptDatabaseName);

            sessionContext.PaymentSessions.Add(new PaymentSession
            {
                Cost = 200,
                PaymentAppointment = "Item1",
                LifeSpanInMinute = 60,
                SessionRegistrationTime = DateTime.Now.AddMinutes(-30),
                Seller = "www.randomshop.com",
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e")
            });

            await sessionContext.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(receiptContext, sessionContext);

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
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e") 
            };

            //Act
            await controller.PayWithCreditCard(paymentInfo);
            var count = receiptContext.Receipts.Count();

            Assert.AreEqual(1, count);
        }

        [Test]
        public async Task Unsuccessful_Payment_Session_Is_Over()
        {
            //Arrange
            var sessionDatabaseName = Guid.NewGuid().ToString();
            var sessionContext = ContextBuilder.BuildPaymentSessionContext(sessionDatabaseName);

            var receiptDatabaseName = Guid.NewGuid().ToString();
            var receiptContext = ContextBuilder.BuildReceiptContext(receiptDatabaseName);

            sessionContext.PaymentSessions.Add(new PaymentSession
            {
                Cost = 200,
                PaymentAppointment = "Item1",
                LifeSpanInMinute = 60,
                SessionRegistrationTime = DateTime.Now.AddMinutes(-90),
                Seller = "www.randomshop.com",
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e")
            });

            await sessionContext.SaveChangesAsync();

            var controller = new PaymentByCreditCardController(receiptContext, sessionContext);

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
                SessionId = new Guid("ebd21f99-46a5-438f-8d6c-7e0a259b278e")
            };

            //Act
            var response = await controller.PayWithCreditCard(paymentInfo);
            var count = receiptContext.Receipts.Count();

            //Assert
            Assert.AreEqual(0, count);
            Assert.NotNull(response is BadRequestObjectResult);
        }
    }
}
