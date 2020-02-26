using System;
namespace PaymentGateway.Models
{
    public class BankPaymentRes
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public long Id { get; set; }

        public BankPaymentRes(long id, string _Message, int Status)
        {
            this.Id = id;
            this.Message = _Message;
            this.Status = Status;
        }

    }
}
