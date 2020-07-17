using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XsollaTestTask1.Models;

namespace XsollaTestTask1Tests.ValidationTests.Tests
{
    public class RequiredAttributeTests
    {
        [Test, TestCaseSource("Cards")]
        public void EmptyRequiredProperties(CreditCard creditCard)
        {
            //arrange
            var context = new ValidationContext(creditCard, null, null);

            //act
            var result = Validator.TryValidateObject(creditCard, context, null);

            //asser
            Assert.IsFalse(result);
        }

        static CreditCard[] Cards =
        {
            new CreditCard { CVV = "123", HolderName ="Alex", ExpirationDate = new DateTime(2021, 10, 1) },
            new CreditCard {Number = "1111222233334444", HolderName ="Alex", ExpirationDate = new DateTime(2021, 10, 1) },
            new CreditCard {Number = "1111222233334444", CVV = "123",  ExpirationDate = new DateTime(2021, 10, 1) },
            new CreditCard {Number = "1111222233334444", CVV = "123",  HolderName ="Alex" },
            new CreditCard(),
        };
    }
}
