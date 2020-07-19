using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XsollaTestTask1.Contexts;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentSessionController : ControllerBase
    {
        private readonly PaymentDbContext context;

        public PaymentSessionController(PaymentDbContext paymentSessionDBContext)
        {
            context = paymentSessionDBContext;
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
            };

            context.PaymentSessions.Add(paymentSession);
            await context.SaveChangesAsync();

            return Ok(paymentSession.SessionId);
        }
    }
}
