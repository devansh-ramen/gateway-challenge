using System;
using Xunit;
using PaymentGateway.Controllers;

namespace PaymentGateway.test
{
    public class PaymentItemValidatorTest
    {
        [Fact]
        public void testValidCreditCard()
        {
            string creditCardNumber = "4512-4567-4567-4567";
            string expiryDate = "11/2022";
            string cvv = "111";

            bool isValid = PaymenttVValidator.IsCreditCardInfoValid(creditCardNumber, expiryDate, cvv);

            Assert.True(isValid);
        }

        [Fact]
        public void testInvalidCreditCard()
        {
            string creditCardNumber = "4512-4567-4567-4567";
            string expiryDate = "11/2019";
            string cvv = "111";

            bool isValid = PaymenttVValidator.IsCreditCardInfoValid(creditCardNumber, expiryDate, cvv);

            Assert.False(isValid);
        }

        [Fact]
        public void testValidAmount()
        {
            float amount = 100;

            bool isValid = PaymenttVValidator.IsAmountValid(amount);

            Assert.True(isValid);
        }

        [Fact]
        public void testInvalidAmount()
        {
            float amount = -10;

            bool isValid = PaymenttVValidator.IsAmountValid(amount);

            Assert.False(isValid);
        }
    }
}
