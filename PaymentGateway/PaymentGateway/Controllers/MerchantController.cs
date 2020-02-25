using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly MerchantContext _context;

        public MerchantController(MerchantContext context)
        {
            _context = context;
        }

        // GET: api/Merchant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Merchant>>> GetMerchant()
        {
            return await _context.Merchant.ToListAsync();
        }

        // GET: api/Merchant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> GetMerchant(string id)
        {
            var merchant = await _context.Merchant.FindAsync(id);

            if (merchant == null)
            {
                return NotFound();
            }

            return merchant;
        }


        // PUT: api/Merchant/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMerchant(string id, Merchant merchant)
        {
            if (id != merchant.MerchantId)
            {
                return BadRequest();
            }

            _context.Entry(merchant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Merchant
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Merchant>> PostMerchant(Merchant merchant)
        {
            // encrypt password before saving into db
            merchant.MerchantPassword = Encryption.EncryptTripleDES(merchant.MerchantPassword);

            _context.Merchant.Add(merchant);
            try
            {
                merchant.MerchantPassword = Encryption.EncryptTripleDES(merchant.MerchantPassword);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MerchantExists(merchant.MerchantId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }


            return CreatedAtAction("GetMerchant", new { id = merchant.MerchantId }, merchant);
        }

        // DELETE: api/Merchant/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Merchant>> DeleteMerchant(string id)
        {
            var merchant = await _context.Merchant.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            _context.Merchant.Remove(merchant);
            await _context.SaveChangesAsync();

            return merchant;
        }

        private bool MerchantExists(string id)
        {
            return _context.Merchant.Any(e => e.MerchantId == id);
        }
    }
}
