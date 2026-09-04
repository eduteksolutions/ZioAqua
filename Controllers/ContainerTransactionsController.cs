using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;
using zioAqua.model.zioAqua.model;

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
        public async Task<ActionResult<IEnumerable<ContainerTransactionMaster>>>
            GetTransactions()
        {
            return await _context.ContainerTransactionMaster
                .Include(x => x.Details)
                .OrderByDescending(x => x.TransactionDate)
                .ToListAsync();
        }

        // GET: api/ContainerTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContainerTransactionMaster>>
            GetTransaction(int id)
        {
            var transaction = await _context.ContainerTransactionMaster
                .Include(x => x.Details)
                .FirstOrDefaultAsync(x => x.TransactionId == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST: api/ContainerTransactions
        // Saves Container + Account Transaction
        [HttpPost]
        public async Task<ActionResult> PostTransaction(
            ContainerTransactionMaster transaction)
        {
            await using var dbTransaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                // -----------------------------------------
                // 1. Set Date
                // -----------------------------------------

                transaction.LUserDt = DateTime.Now;

                if (transaction.TransactionDate == default)
                {
                    transaction.TransactionDate = DateTime.Now;
                }

                // -----------------------------------------
                // 2. Save Container Master
                // -----------------------------------------

                _context.ContainerTransactionMaster.Add(transaction);

                await _context.SaveChangesAsync();

                // -----------------------------------------
                // 3. Save Container Details
                // -----------------------------------------

                decimal totalAmount = 0;

                if (transaction.Details != null &&
                    transaction.Details.Count > 0)
                {
                    foreach (var detail in transaction.Details)
                    {
                        detail.TransactionId =
                            transaction.TransactionId;

                        // Calculate amount automatically
                        detail.Amount =
                            detail.Qty * detail.Rate;

                        totalAmount += detail.Amount;

                        _context.ContainerTransactionDetail.Add(detail);
                    }

                    await _context.SaveChangesAsync();
                }

                // -----------------------------------------
                // 4. Create Account Transaction
                // -----------------------------------------

                if (totalAmount > 0)
                {
                    var accTransaction =
                        new AccTransactionMaster
                        {
                            TransactionNo =
                                transaction.TransactionNo,

                            TransactionDate =
                                transaction.TransactionDate,

                            TransactionType =
                                transaction.TransactionType,

                            BusinessId =
                                transaction.BusinessId,

                            Remark =
                                transaction.Remark,

                            LoginName =
                                transaction.LoginName,

                            LUserDt =
                                DateTime.Now
                        };

                    _context.AccTransactionMaster
                        .Add(accTransaction);

                    // Save first to get Account TransactionId
                    await _context.SaveChangesAsync();

                    // -----------------------------------------
                    // 5. Shop Debit
                    // -----------------------------------------

                    var shopLedger =
                        new AccTransactionDetail
                        {
                            TransactionId =
                                accTransaction.TransactionId,

                            // ShopId should contain aCode
                            aCode =
                                transaction.ShopId,

                            Debit =
                                totalAmount,

                            Credit = 0
                        };

                    // -----------------------------------------
                    // 6. Water Sales Credit
                    // -----------------------------------------

                    var waterSalesLedger =
                        new AccTransactionDetail
                        {
                            TransactionId =
                                accTransaction.TransactionId,

                            // Water Sales ledger
                            aCode = 49,

                            Debit = 0,

                            Credit =
                                totalAmount
                        };

                    _context.AccTransactionDetail.Add(shopLedger);

                    _context.AccTransactionDetail.Add(
                        waterSalesLedger);

                    await _context.SaveChangesAsync();
                }

                // -----------------------------------------
                // 7. Commit Everything
                // -----------------------------------------

                await dbTransaction.CommitAsync();

                return CreatedAtAction(
                    nameof(GetTransaction),
                    new
                    {
                        id = transaction.TransactionId
                    },
                    new
                    {
                        Status = true,

                        Message =
                            "Container and Account transaction saved successfully",

                        ContainerTransactionId =
                            transaction.TransactionId,

                        TotalAmount =
                            totalAmount
                    });
            }
            catch (Exception ex)
            {
                // -----------------------------------------
                // Rollback Everything
                // -----------------------------------------

                await dbTransaction.RollbackAsync();

                return BadRequest(new
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }

        // PUT: api/ContainerTransactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(
            int id,
            ContainerTransactionMaster transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State =
                EntityState.Modified;

            _context.Entry(transaction)
                .Property(x => x.LUserDt)
                .IsModified = false;

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

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ContainerTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction =
                await _context.ContainerTransactionMaster
                    .FindAsync(id);

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
            return _context.ContainerTransactionMaster
                .Any(e => e.TransactionId == id);
        }
    }
}