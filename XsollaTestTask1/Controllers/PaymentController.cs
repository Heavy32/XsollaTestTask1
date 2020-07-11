using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XsollaTestTask1.Contexts;
using XsollaTestTask1.Models;
using System.Text.Json;

namespace XsollaTestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentSessionDBContext paymentSessionDBContext;
        private readonly string sellerName = "www.randomshop.com";

        public PaymentController(PaymentSessionDBContext paymentSessionDBContext)
        {
            this.paymentSessionDBContext = paymentSessionDBContext;
        }

        [HttpGet]
        public ActionResult Test()
        {
            return Ok("Test");
        }

        [HttpPost]
        public ActionResult CreateSession(int sum, string appointment)
        {
            //validation code
            //
            //

            PaymentSession paymentSession = new PaymentSession
            {
                Cost = sum,
                SessionId = Guid.NewGuid(),
                PaymentAppointment = appointment,
                SessionRegistrationTime = DateTime.Now,
                LifeSpanInMinute = 60,
                Seller = sellerName
            };

            paymentSessionDBContext.Add<PaymentSession>(paymentSession);
            paymentSessionDBContext.SaveChanges();

            return Ok(paymentSession.SessionId);
        }

        public ActionResult Payment(CreditCard creditCard, Guid sessionId)
        {
            //validation code
            //
            //

            PaymentSession session = paymentSessionDBContext.PaymentSessions.Find(sessionId);

            Receipt receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CustomerName = creditCard.HolderName,
                Good = session.PaymentAppointment,
                OperationTime = DateTime.Now,
                Seller = session.Seller
            };

            //code to add the receipt to db

            return Ok();
        }
    }
}
