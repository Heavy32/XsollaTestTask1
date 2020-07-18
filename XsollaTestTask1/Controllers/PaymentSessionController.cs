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

        public PaymentSessionController(PaymentSessionDBContext paymentSessionDBContext)
        {
            this.paymentSessionDBContext = paymentSessionDBContext;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<ActionResult<Guid>> CreateSession([FromBody]PaymentSessionInputInfo info)
        {
            if(info == null)
            {
                return BadRequest();
            }

            PaymentSession paymentSession = new PaymentSession
            {
                Cost = info.Sum,
                SessionId = Guid.NewGuid(),
                PaymentAppointment = info.PaymentAppointement,
                SessionRegistrationTime = DateTime.Now,
                LifeSpanInMinute = 60,
                Seller = info.Seller
            };

            paymentSessionDBContext.PaymentSessions.Add(paymentSession);
            await paymentSessionDBContext.SaveChangesAsync();

            return Ok(paymentSession.SessionId);
        }
    }
}
