using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XsollaTestTask1.Contexts;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentByCreditCardController : ControllerBase
    {
        private readonly PaymentDbContext context;

        public PaymentByCreditCardController(PaymentDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Receipt), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Receipt>>> GetAllPaymentHistory(int periodInDays)
        {
            DateTime selectionStartTime = DateTime.Now.AddDays(-periodInDays);
            var receipts = await context.Receipts.Where(receipt => receipt.OperationTime >= selectionStartTime).ToListAsync();
            return receipts;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PayByCreditCard(PaymentInputInfo info)
        {
            IPayment paymentByCreditCard = new PaymentByCreditCard 
            { 
                Context = context,
                Info = info
            };
            paymentByCreditCard.PaymentStep();

            SendNotificationDecorator sendNotification = new SendNotificationDecorator(paymentByCreditCard);
            sendNotification.PaymentStep();

            return Ok();
        }
    }
}
