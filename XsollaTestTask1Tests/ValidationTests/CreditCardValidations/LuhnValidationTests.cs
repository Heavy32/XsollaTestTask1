using NUnit.Framework;
using XsollaTestTask1.Models;

namespace XsollaTestTask1.ValidationTests.Tests
{

    public class LuhnValidationTests
    {
        [TestCase("67619600 0494451892")]
        [TestCase("67628038 8885503265")]
        [TestCase("4561 2612 1234 5467")]
        [TestCase("4123 4567 8901 2349")]
        [TestCase("0000 0000 0000 0000")]
        public void LuhnValidationTrueTest(string cardNumber)
        {
            CreditCard card = new CreditCard { Number = cardNumber };
            Assert.AreEqual(true, card.LuhnValidation());
        }

        [TestCase("4561 2612 1234 5464")]
        [TestCase("2200 3814 2733 0082")]
        [TestCase("3713 317306 61007")]
        [TestCase("5561 2612 1234 5467")]
        public void LuhnValidationFalseTest(string cardNumber)
        {
            CreditCard card = new CreditCard { Number = cardNumber };
            Assert.AreEqual(false, card.LuhnValidation());
        }
    }
}