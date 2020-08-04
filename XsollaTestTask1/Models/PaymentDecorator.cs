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

namespace XsollaTestTask1.Models
{
    public interface IPayment
    {
        public PaymentDbContext Context { get; set; }
        public PaymentInputInfo Info { get; set; }
        public Receipt Receipt { get; set; }

        public void PaymentStep();
    }

    public class PaymentByCreditCard : IPayment
    {
        public PaymentDbContext Context { get; set; }
        public PaymentInputInfo Info { get; set; }
        public Receipt Receipt { get; set; }

        public void PaymentStep()
        {
            var paymentSession = Context.PaymentSessions.FirstOrDefault(session => session.SessionId == Info.SessionId);

            Receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CustomerName = Info.Card.HolderName,
                Product = paymentSession.PaymentAppointment,
                OperationTime = DateTime.Now,
                Seller = Info.Seller,
                Cost = paymentSession.Cost
            };

            Context.Receipts.Add(Receipt);
            Context.SaveChanges();
        }
    }

    public abstract class PaymentDecorator : IPayment
    {
        public PaymentDbContext Context { get; set; }
        public PaymentInputInfo Info { get; set; }
        public Receipt Receipt { get; set; }

        private readonly IPayment paymentElement;

        public PaymentDecorator(IPayment paymentElement)
        {
            this.paymentElement = paymentElement;
            Info = paymentElement.Info;
            Context = paymentElement.Context;
            Receipt = paymentElement.Receipt;
        }

        public virtual void PaymentStep()
        {
            paymentElement.PaymentStep();
        }
    }

    public class SendNotificationDecorator : PaymentDecorator
    {
        public SendNotificationDecorator(IPayment paymentElement) : base(paymentElement)
        {
        }

        public override void PaymentStep()
        {
            base.PaymentStep();
            SendNotification(base.Receipt);
        }

        private async void SendNotification(Receipt receipt)
        {
            var url = receipt.CustomerName;
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
