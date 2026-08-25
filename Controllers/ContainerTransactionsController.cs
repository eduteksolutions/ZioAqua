using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
  

    
        [Route("api/[controller]")]
        [ApiController]
        public class ContainerTransactionsController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public ContainerTransactionsController(ApplicationDbContext context)
            {
                _context = context;
            }

            // GET: api/ContainerTransactions
            [HttpGet]
            public async Task<ActionResult<IEnumerable<ContainerTransactionMaster>>> GetTransactions()
            {
                return await _context.ContainerTransactionMaster.ToListAsync();
            }

            // GET: api/ContainerTransactions/5
            [HttpGet("{id}")]
            public async Task<ActionResult<ContainerTransactionMaster>> GetTransaction(int id)
            {
                var transaction = await _context.ContainerTransactionMaster.FindAsync(id);

                if (transaction == null)
                {
                    return NotFound();
                }

                return transaction;
            }

        // POST: api/ContainerTransactions
        // POST: api/ContainerTransactions
        [HttpPost]
        public async Task<ActionResult<ContainerTransactionMaster>> PostTransaction(ContainerTransactionMaster transaction)
        {
            try
            {
                transaction.LUserDt = DateTime.Now;

                // Add Master
                _context.ContainerTransactionMaster.Add(transaction);

                // Save master first to get TransactionId
                await _context.SaveChangesAsync();


                // Add Details
                if (transaction.Details != null && transaction.Details.Count > 0)
                {
                    foreach (var detail in transaction.Details)
                    {
                        detail.TransactionId = transaction.TransactionId;

                        _context.ContainerTransactionDetail.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                }


                return CreatedAtAction(
                    nameof(GetTransaction),
                    new { id = transaction.TransactionId },
                    transaction
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/ContainerTransactions/5
        [HttpPut("{id}")]
            public async Task<IActionResult> PutTransaction(int id, ContainerTransactionMaster transaction)
            {
                if (id != transaction.TransactionId)
                {
                    return BadRequest();
                }

                _context.Entry(transaction).State = EntityState.Modified;

                // Prevent overriding LUserDt on update if needed, or update it
                _context.Entry(transaction).Property(x => x.LUserDt).IsModified = false;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(id))
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

            // DELETE: api/ContainerTransactions/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteTransaction(int id)
            {
                var transaction = await _context.ContainerTransactionMaster.FindAsync(id);
                if (transaction == null)
                {
                    return NotFound();
                }

                _context.ContainerTransactionMaster.Remove(transaction);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            private bool TransactionExists(int id)
            {
                return _context.ContainerTransactionMaster.Any(e => e.TransactionId == id);
            }
        }
    }

