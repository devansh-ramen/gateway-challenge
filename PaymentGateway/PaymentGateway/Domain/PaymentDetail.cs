using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Domain
{
    public class PaymentDetail
    {
        public long Id { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string MerchantPassword { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public string DateCreated { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public float Fee { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Cvv { get; set; }

        public enum Status
        {
            Failed = 001,
            Pending = 002,
            Success = 200,
        }

        public PaymentDetail()
        {
        }

    }

}
