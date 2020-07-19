using System;
using System.Threading.Tasks;
using XsollaTestTask1.Controllers;
using XsollaTestTask1.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace XsollaTestTask1Tests.ControllerTests
{
    public class PaymentSessionControllerTests
    {
        [Test]
        public async Task Create_New_PaymentSession_Return_New_Element()
        {
            //Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(databaseName);         
            var controller = new PaymentSessionController(context);

            //Action
            var result = await controller.CreateSession(new PaymentSessionInputInfo { PaymentAppointement = "Beer", Sum = 50 });

            //Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public async Task Createy_Null_PaymentSession_Return_Bad_Request()
        {
            //arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = ContextBuilder.BuildContext(databaseName);
            var controller = new PaymentSessionController(context);

            //act
            var result = await controller.CreateSession(null);
            var statusCode = result.Result as StatusCodeResult; 

            //Assert
            Assert.AreEqual(400, statusCode.StatusCode);
        }
    }
}
