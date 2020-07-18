using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly ReceiptDBContext receiptDBContext;
        private readonly PaymentSessionDBContext paymentSessionDBContext;

        public PaymentByCreditCardController(ReceiptDBContext receiptDBContext, PaymentSessionDBContext paymentSessionDBContext)
        {
            this.receiptDBContext = receiptDBContext;
            this.paymentSessionDBContext = paymentSessionDBContext;
        }


        [HttpGet]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(Receipt), 200)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<Receipt>>> GetAllPaymentHistory(int periodInDays)
        {
            DateTime selectionStartTime = DateTime.Now.AddDays(-periodInDays);
            var receipts = await receiptDBContext.Receipts.Where(receipt => receipt.OperationTime >= selectionStartTime).ToListAsync();
            return receipts;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PayWithCreditCard(PaymentInputInfo info)
        {            
            var paymentSession = await paymentSessionDBContext.PaymentSessions.FirstOrDefaultAsync(session => session.SessionId == info.SessionId);

            if(paymentSession.SessionRegistrationTime.AddMinutes(paymentSession.LifeSpanInMinute) < DateTime.Now)
            {            
                return BadRequest("Payment session is out of time");
            }

            Receipt receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CustomerName = info.card.HolderName,
                Product = paymentSession.PaymentAppointment,
                OperationTime = DateTime.Now,
                Seller = paymentSession.Seller,
                Cost = paymentSession.Cost              
            };

            paymentSessionDBContext.PaymentSessions.Remove(paymentSession);
            receiptDBContext.Receipts.Add(receipt);
            await receiptDBContext.SaveChangesAsync();

            await SendNotificationToShop(receipt);

            return Ok($"Congratulations, {info.card.HolderName}, you are a happy owner of {paymentSession.PaymentAppointment}!");
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
            await client.PostAsync(url, content);
        }
    }
}
