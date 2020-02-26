using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentGateway.Domain;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentItemsController : ControllerBase
    {
        private readonly PaymentContext _paymentContext;
        private readonly MerchantContext _merchantContext;

        public PaymentItemsController(PaymentContext paymentContext, MerchantContext merchantContext)
        {
            _paymentContext = paymentContext;
            _merchantContext = merchantContext;
        }

        // GET: api/PaymentItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentItem>>> GetPaymentItems()
        {
            return await _paymentContext.PaymentItems.ToListAsync();
        }

        // GET: api/PaymentItems/{MERCHANT_ID} retrieve all payment items for a merchant
        [HttpGet("merchantId/{merchantId}")]
        public async Task<ActionResult<IEnumerable<PaymentReceipt>>> GetPaymentItemsForMerchant(string merchantId)
        {
            var paymentItems = await _paymentContext.PaymentItems.ToListAsync();

            var filteredItems = paymentItems.FindAll(paymentItem => paymentItem.MerchantId == merchantId);

            List<PaymentReceipt> listReceipts = new List<PaymentReceipt>();

            filteredItems.ForEach(paymentItem =>
            {
                listReceipts.Add(new PaymentReceipt(paymentItem.Id, paymentItem.MerchantId, paymentItem.CustomerId, paymentItem.DateCreated, paymentItem.Amount, paymentItem.Fee, paymentItem.Currency, paymentItem.Status));
            });

            return listReceipts;

        }

        // GET: api/PaymentItems
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentReceipt>> GetPaymentItem(long id)
        {
            var paymentItem = await _paymentContext.PaymentItems.FindAsync(id);

            if (paymentItem == null)
            {
                return NotFound();
            }

            PaymentReceipt paymentReceipt = new PaymentReceipt(paymentItem.Id, paymentItem.MerchantId, paymentItem.CustomerId, paymentItem.DateCreated, paymentItem.Amount, paymentItem.Fee, paymentItem.Currency, paymentItem.Status);

            return paymentReceipt;
        }


        // PUT: api/PaymentItems/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentItem(long id, PaymentItem paymentItem)
        {
            if (id != paymentItem.Id)
            {
                return BadRequest();
            }

            _paymentContext.Entry(paymentItem).State = EntityState.Modified;

            try
            {
                await _paymentContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentItemExists(id))
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

        // POST: api/PaymentItems
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PaymentReceipt>> PostPaymentItem(PaymentItem paymentItem)
        {

            if (!PaymentValidator.IsAmountValid(paymentItem.Amount))
            {
                return BadRequest(new { message = "Invalid Amount" });
            }
            else if (!PaymentValidator.IsCreditCardInfoValid(paymentItem.CardNo, paymentItem.ExpiryDate, paymentItem.Cvv))
            {
                return BadRequest(new { message = "Invalid card number" });
            }
            else if (PaymentItemExists(paymentItem.Id))
            {
                return BadRequest(new { message = "Payment already processed" });
            }

            // check if merchant exist
            var merchant = _merchantContext.Merchant.Find(paymentItem.MerchantId);
            if (merchant == null)
            {
                return BadRequest(new { message = "Merchant does not exist" });
            }

            // call bank api to submit payment
            string apiUrl = "https://paymentgatewaydevansh.azurewebsites.net/api/BankSimulate";
            HttpResponseMessage response;


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var bankPaymentReq = new BankPaymentReq();
                bankPaymentReq.Id = paymentItem.Id;
                bankPaymentReq.Amount = paymentItem.Amount;
                bankPaymentReq.CardNo = paymentItem.CardNo;
                bankPaymentReq.MerchantId = paymentItem.MerchantId;

                // Serialize our concrete class into a JSON String
                var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(bankPaymentReq));// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

                response = await client.PostAsync(apiUrl, httpContent);
            }

            paymentItem.CardNo = Encryption.EncryptTripleDES(paymentItem.CardNo);
            paymentItem.Cvv = Encryption.EncryptTripleDES(paymentItem.Cvv);
            paymentItem.ExpiryDate = Encryption.EncryptTripleDES(paymentItem.ExpiryDate);

            paymentItem.DateCreated = DateTime.UtcNow.ToLongDateString();

            if (response.StatusCode.Equals(HttpStatusCode.OK))
            {
                var data = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(data);

                paymentItem.Status = "success";
                paymentItem.Fee = (float)0.03 * paymentItem.Amount;

                _paymentContext.PaymentItems.Add(paymentItem);
                await _paymentContext.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Payment proccessed");

                PaymentReceipt paymentReceipt = new PaymentReceipt(paymentItem.Id, paymentItem.MerchantId, paymentItem.CustomerId, paymentItem.DateCreated, paymentItem.Amount, paymentItem.Fee, paymentItem.Currency, paymentItem.Status);

              //  return CreatedAtAction("GetPaymentItem", new { id = paymentItem.Id }, paymentItem);
                return paymentReceipt;
            }
            else
            {
                paymentItem.Status = "failed";
                _paymentContext.PaymentItems.Add(paymentItem);
                await _paymentContext.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Payment failed");

                return BadRequest(new { message = "Bank could not process payment." });
            }
        }

        private HttpContent JsonOptions(BankPaymentReq bankPaymentReq)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/PaymentItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentItem>> DeletePaymentItem(long id)
        {
            var paymentItem = await _paymentContext.PaymentItems.FindAsync(id);
            if (paymentItem == null)
            {
                return NotFound();
            }

            _paymentContext.PaymentItems.Remove(paymentItem);
            await _paymentContext.SaveChangesAsync();

            return paymentItem;
        }

        private bool PaymentItemExists(long id)
        {
            return _paymentContext.PaymentItems.Any(e => e.Id == id);
        }
    }
}
