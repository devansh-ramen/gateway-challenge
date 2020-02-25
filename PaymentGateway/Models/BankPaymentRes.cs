using System;
namespace PaymentGateway.Models
{
    public class BankPaymentRes
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public BankPaymentRes(string _Message, int Status)
        {
            this.Message = _Message;
            this.Status = Status;
        }

    }
}
