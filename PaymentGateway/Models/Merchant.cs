using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Models
{
    public class Merchant
    {
        [Required]
        public string MerchantId { get; set; }
        [Required]
        public string MerchantPassword { get; set; }
        [Required]
        public string AccountNum { get; set; }
        [Required]
        public string Location { get; set; }

        public Merchant()
        {
        }
    }
}
