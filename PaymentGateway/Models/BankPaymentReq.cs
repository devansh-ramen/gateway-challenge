using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class BankPaymentReq
    {
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public float Amount { get; set; }

        public BankPaymentReq(string merchantId, string cardNo, float amount)
        {
            MerchantId = merchantId;
            CardNo = cardNo;
            Amount = amount;
        }

        public BankPaymentReq()
        {
        }
    }
}
