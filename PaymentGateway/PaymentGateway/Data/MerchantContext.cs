using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Models;

    public class MerchantContext : DbContext
    {
        public MerchantContext (DbContextOptions<MerchantContext> options)
            : base(options)
        {
        }

        public DbSet<PaymentGateway.Models.Merchant> Merchant { get; set; }
    }
