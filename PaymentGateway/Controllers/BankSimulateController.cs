using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankSimulateController : ControllerBase
    {
        [HttpPost]
        public ActionResult<BankPaymentRes> BankSimulateResponse(BankPaymentReq bankPaymentReq)
        {
            if (bankPaymentReq.Amount > 10000)
            {
                return BadRequest(new { message = "Insufficient Funds" });
            } else
            {
                return new BankPaymentRes(bankPaymentReq.Id, "Transaction completed", 200);
            }
        }

    }
}
