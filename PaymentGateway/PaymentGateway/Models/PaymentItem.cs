using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class PaymentItem
    {
        public long Id { get; set; }
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string CustomerId { get; set; }

        public string DateCreated { get; set; }
        [Required]
        public string CardNo { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public float Fee { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Cvv { get; set; }

        public string Status { get; set; }

        public PaymentItem()
        {
        }

    }
   
}
