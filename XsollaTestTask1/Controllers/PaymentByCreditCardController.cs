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
            var paymentSession = await context.PaymentSessions.FirstOrDefaultAsync(session => session.SessionId == info.SessionId);

            if(paymentSession.SessionRegistrationTime.AddMinutes(paymentSession.LifeSpanInMinute) < DateTime.Now)
            {            
                return BadRequest("Payment session is out of time");
            }

            Receipt receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CustomerName = info.Card.HolderName,
                Product = paymentSession.PaymentAppointment,
                OperationTime = DateTime.Now,
                Seller = info.Seller,
                Cost = paymentSession.Cost              
            };

            context.PaymentSessions.Remove(paymentSession);
            context.Receipts.Add(receipt);
            await context.SaveChangesAsync();

            await SendNotificationToShop(receipt);

            return Ok($"Congratulations, {info.Card.HolderName}, you are a happy owner of {paymentSession.PaymentAppointment}!");
        }

        private async Task SendNotificationToShop(Receipt receipt)
        {
            var url = receipt.Seller;
            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
            {
                { "Customer", receipt.CustomerName },
                { "ProductSold", receipt.Product }
            };

            var content = new FormUrlEncodedContent(values);

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                await client.PostAsync(uri, content);
            }
        }
    }
}
