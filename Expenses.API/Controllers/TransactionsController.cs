using Expenses.API.Data;
using Expenses.API.Data.Services;
using Expenses.API.Dtos;
using Expenses.API.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Expenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController(ITransactionsService transactionService) : ControllerBase
    {

        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifierClaim))
                return BadRequest("Could not get the user id");

            if (!int.TryParse(nameIdentifierClaim, out int userId))
                return BadRequest();

            var allTransactions = transactionService.GetAll(userId);
            return Ok(allTransactions);
        }

        //For async methods
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var allTransactions = await context.Transactions.ToListAsync();
        //    return Ok(allTransactions);
        //}

        [HttpGet("Details/{id}")]
        public IActionResult Get(int id)
        {
            var transactionDb = transactionService.GetById(id);
            if (transactionDb == null)
                return NotFound();

            return Ok(transactionDb);
        }


        [HttpGet("Details")]
        public IActionResult GetNew(int id)
        {
            var transactionDb = transactionService.GetById(id);
            if (transactionDb == null)
                return NotFound();

            return Ok(transactionDb);
        }

        [HttpPost("Create")]
        public IActionResult CreateTransaction([FromBody] PostTransactionDto payload)
        {
            var nameIdentifierClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(nameIdentifierClaim))
                return BadRequest("Could not get the user id");

            if (!int.TryParse(nameIdentifierClaim, out int userId))
                return BadRequest();

            var newTransaction = transactionService.Add(payload, userId);
            return Ok(newTransaction);
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateTransaction(int id, [FromBody] PutTransactionDto payload)
        {
            var UpdatedTransaction = transactionService.Update(id, payload);

            if (UpdatedTransaction == null)
                return NotFound();

            return Ok(UpdatedTransaction);

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            transactionService.Delete(id);

            return Ok();
        }

    }
}
