using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XsollaTestTask1.Models;

namespace XsollaTestTask1Tests.ValidationTests.CreditCardValidation
{
    public class SpecificValidation
    {
        [TestCase("1111 2222 3333 44")]
        [TestCase("134 22232 323433 442345")]
        [TestCase("133454 2212532 32456433 445")]
        [TestCase("1337 633 50541")]
        [TestCase("80085 666 777 007")]
        [TestCase("00101 1100 00001 001")]
        public void WrongCreditCardNumbersCount(string number)
        {
            //arrange
            CreditCard creditCard = new CreditCard
            {
                Number = number,
                CVV = "123",
                ExpirationDate = new DateTime(2021, 10, 1),
                HolderName = "Alex"
            };

            var context = new ValidationContext(creditCard);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(creditCard, context, results);
            var errorMessage = results[0].ErrorMessage;

            //assert
            Assert.AreEqual(errorMessage, "Incorrect numbers' count");
        }

        [TestCase("aaaa bbbb cccc dddd")]
        [TestCase("tata 123b dfcc aaza")]
        [TestCase("hello hello hello")]
        [TestCase("1111 2222 3333 444z")]
        public void WrongSymbolsInCardNumbersCount(string number)
        {
            //arrange
            CreditCard creditCard = new CreditCard
            {
                Number = number,
                CVV = "123",
                ExpirationDate = new DateTime(2021, 10, 1),
                HolderName = "Alex"
            };

            var context = new ValidationContext(creditCard);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(creditCard, context, results);
            var errorMessage = results[0].ErrorMessage;

            //assert
            Assert.AreEqual(errorMessage, "Card number contains wrong symbols");
        }

        [TestCaseSource("wrongExpirationDates")]
        public void ExpirationDateIsOver(DateTime expirationDate)
        {
            //arrange
            CreditCard creditCard = new CreditCard
            {
                Number = "1111 2222 3333 4444",
                CVV = "123",
                ExpirationDate = expirationDate,
                HolderName = "Alex"
            };

            var context = new ValidationContext(creditCard);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(creditCard, context, results);
            var errorMessage = results[0].ErrorMessage;

            //assert
            Assert.AreEqual(errorMessage, "Card is not support due to the expiration date has been out");
        }

        private static readonly DateTime[] wrongExpirationDates =
        {
            new DateTime(2019, 7, 23),
            new DateTime(2018, 6, 26),
            new DateTime(2017, 3, 17),
            new DateTime(2019, 11, 19),
            new DateTime(2020, 4, 5),
            new DateTime(2011, 12, 6)
        };

        [TestCase("abc")]
        [TestCase("zzz")]
        [TestCase("yyy")]
        [TestCase("wtytry")]
        [TestCase("923r")]
        [TestCase("p3r")]
        [TestCase("lol")]
        public void CVVWrongInput(string cvv)
        {
            //arrange
            CreditCard creditCard = new CreditCard
            {
                Number = "1111 2222 3333 4444",
                CVV = cvv,
                ExpirationDate = new DateTime(2021, 11, 19),
                HolderName = "Alex"
            };

            var context = new ValidationContext(creditCard);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(creditCard, context, results);
            var errorMessage = results[0].ErrorMessage;

            //assert
            Assert.AreEqual(errorMessage, "CVV/CVC contains wrong symbols");
        }

        [TestCase("1111")]
        [TestCase("11112222")]
        [TestCase("0")]
        [TestCase("00")]
        [TestCase("666777")]
        [TestCase("8923409852034786502198364509827")]
        [TestCase("07896577094578095470954790479")]
        public void CVVWrongNumbersCount(string cvv)
        {
            //arrange
            CreditCard creditCard = new CreditCard
            {
                Number = "1111 2222 3333 4444",
                CVV = cvv,
                ExpirationDate = new DateTime(2021, 11, 19),
                HolderName = "Alex"
            };

            var context = new ValidationContext(creditCard);
            var results = new List<ValidationResult>();

            //act
            Validator.TryValidateObject(creditCard, context, results);
            var errorMessage = results[0].ErrorMessage;

            //assert
            Assert.AreEqual(errorMessage, "CVV/CVC must have 3 symbols");
        }
    }
}
