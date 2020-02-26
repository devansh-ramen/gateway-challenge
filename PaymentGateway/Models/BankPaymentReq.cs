using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class BankPaymentReq
    {

        [Required]
        public long Id { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public float Amount { get; set; }

        public BankPaymentReq(long id, string merchantId, string cardNo, float amount)
        {
            Id = id;
            MerchantId = merchantId;
            CardNo = cardNo;
            Amount = amount;
        }

        public BankPaymentReq()
        {
        }
    }
}
