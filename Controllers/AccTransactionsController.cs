using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;
using zioAqua.model.zioAqua.model;

namespace zioAqua.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccTransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccTransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AccTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccTransactionMaster>>> GetTransactions()
        {
            var data = await _context.AccTransactionMaster
                .Include(x => x.Details)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/AccTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccTransactionMaster>> GetTransaction(int id)
        {
            var transaction = await _context.AccTransactionMaster
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.TransactionId == id);

            if (transaction == null)
                return NotFound();

            return Ok(transaction);
        }

        // POST: api/AccTransactions
        [HttpPost]
        public async Task<ActionResult<AccTransactionMaster>> PostTransaction(
            AccTransactionMaster transaction)
        {
            try
            {
                transaction.LUserDt = DateTime.Now;

                // Add Master
                _context.AccTransactionMaster.Add(transaction);

                await _context.SaveChangesAsync();

                // Add Details
                if (transaction.Details != null &&
                    transaction.Details.Count > 0)
                {
                    foreach (var detail in transaction.Details)
                    {
                        detail.TransactionId = transaction.TransactionId;

                        _context.AccTransactionDetail.Add(detail);
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

        // PUT: api/AccTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(
            int id,
            AccTransactionMaster transaction)
        {
            if (id != transaction.TransactionId)
                return BadRequest();

            var existing = await _context.AccTransactionMaster
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.TransactionId == id);

            if (existing == null)
                return NotFound();

            existing.TransactionNo = transaction.TransactionNo;
            existing.TransactionDate = transaction.TransactionDate;
            existing.TransactionType = transaction.TransactionType;
            existing.BusinessId = transaction.BusinessId;
            existing.Remark = transaction.Remark;
            existing.LoginName = transaction.LoginName;

            // Remove old details
            _context.AccTransactionDetail.RemoveRange(existing.Details);

            // Add new details
            if (transaction.Details != null)
            {
                foreach (var detail in transaction.Details)
                {
                    detail.TransactionId = id;
                    detail.TransactionDetailId = 0;

                    _context.AccTransactionDetail.Add(detail);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/AccTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.AccTransactionMaster
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.TransactionId == id);

            if (transaction == null)
                return NotFound();

            _context.AccTransactionMaster.Remove(transaction);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.AccTransactionMaster
                .Any(x => x.TransactionId == id);
        }
    }
}