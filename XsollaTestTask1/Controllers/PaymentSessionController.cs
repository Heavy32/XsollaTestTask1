using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XsollaTestTask1.Contexts;
using XsollaTestTask1.Models;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace XsollaTestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSessionController : ControllerBase
    {
        private readonly PaymentSessionDBContext paymentSessionDBContext;
        private readonly string sellerName = "www.randomshop.com";

        public PaymentSessionController(PaymentSessionDBContext paymentSessionDBContext)
        {
            this.paymentSessionDBContext = paymentSessionDBContext;
        }

        [HttpPost]
        public ActionResult CreateSession([FromBody]PaymentSessionInputInfo info)
        {
            PaymentSession paymentSession = new PaymentSession
            {
                Cost = info.Sum,
                SessionId = Guid.NewGuid(),
                PaymentAppointment = info.PaymentAppointement,
                SessionRegistrationTime = DateTime.Now,
                LifeSpanInMinute = 60,
                Seller = sellerName
            };

            paymentSessionDBContext.PaymentSessions.Add(paymentSession);
            paymentSessionDBContext.SaveChanges();

            return Ok(paymentSession.SessionId);
        }

        [HttpGet]
        public ActionResult GetSessions()
        {
            return Ok(paymentSessionDBContext.PaymentSessions);
        }
    }
}
