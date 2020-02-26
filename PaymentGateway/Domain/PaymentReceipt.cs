using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Domain
{
    public class PaymentReceipt
    {
        public long Id { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string CustomerId { get; set; }

        public string DateCreated { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public float Fee { get; set; }
        [Required]
        public string Currency { get; set; }

        public string Status { get; set; }

        public PaymentReceipt()
        {
        }

        public PaymentReceipt(long id, string merchantId, string customerId, string dateCreated, float amount, float fee, string currency, string status)
        {
            Id = id;
            MerchantId = merchantId;
            CustomerId = customerId;
            DateCreated = dateCreated;
            Amount = amount;
            Fee = fee;
            Currency = currency;
            Status = status;
        }
    }

}
