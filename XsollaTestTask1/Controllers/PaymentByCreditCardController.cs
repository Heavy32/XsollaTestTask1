using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPost]
        public ActionResult PayWithCreditCard(PaymentInputInfo info)
        {            
            var paymentSession = paymentSessionDBContext.PaymentSessions.Find(info.SessionId);

            if(paymentSession.SessionRegistrationTime.AddMinutes(paymentSession.LifeSpanInMinute) > DateTime.Now)
            {
                paymentSessionDBContext.PaymentSessions.Remove(paymentSession);
                return BadRequest("Payment session is out of time");
            }

            Receipt receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CustomerName = info.card.HolderName,
                Good = paymentSession.PaymentAppointment,
                OperationTime = DateTime.Now,
                Seller = paymentSession.Seller
            };

            receiptDBContext.Receipts.Add(receipt);
            return Ok($"Congratulations, {info.card.HolderName}, you are a happy owner of {paymentSession.PaymentAppointment}!");
        }

        private async void SendNotificationToShop(Receipt receipt)
        {
            HttpClient client = new HttpClient();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendJoin(' ', new string[] { receipt.CustomerName, " has bought a ", receipt.Good });

            var values = stringBuilder.ToString();

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

            var responseString = await response.Content.ReadAsStringAsync();
        }
    }
}
