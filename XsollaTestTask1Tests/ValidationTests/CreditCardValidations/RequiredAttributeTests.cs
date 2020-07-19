using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using XsollaTestTask1.Models;

namespace XsollaTestTask1Tests.ValidationTests.Tests
{
    public class RequiredAttributeTests
    {
        [TestCaseSource("Cards")]
        public void EmptyRequiredProperties(CreditCard creditCard)
        {
            //Arrange
            var context = new ValidationContext(creditCard, null, null);

            //Act
            var result = Validator.TryValidateObject(creditCard, context, null);

            //Assert
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
