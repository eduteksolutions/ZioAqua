using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessMasterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BusinessMasterController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/BusinessMaster
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessMaster>>> GetBusiness()
        {
            var data = await _context.BusinessMaster
                .OrderByDescending(x => x.BusinessId)
                .ToListAsync();

            return Ok(data);
        }


        // GET: api/BusinessMaster/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessMaster>> GetBusiness(int id)
        {
            var business = await _context.BusinessMaster
                .FirstOrDefaultAsync(x => x.BusinessId == id);

            if (business == null)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Business Not Found"
                });
            }

            return Ok(business);
        }


        // POST: api/BusinessMaster
        [HttpPost]
        public async Task<ActionResult<BusinessMaster>> PostBusiness(BusinessMaster business)
        {
            try
            {
                business.LUserDt = DateTime.Now;

                _context.BusinessMaster.Add(business);

                await _context.SaveChangesAsync();


                return CreatedAtAction(
                    nameof(GetBusiness),
                    new { id = business.BusinessId },
                    new
                    {
                        Status = true,
                        Message = "Business Created Successfully",
                        BusinessId = business.BusinessId
                    });
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


        // PUT: api/BusinessMaster/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusiness(int id, BusinessMaster business)
        {
            if (id != business.BusinessId)
            {
                return BadRequest();
            }


            try
            {
                business.LUserDt = DateTime.Now;

                _context.Entry(business).State = EntityState.Modified;

                await _context.SaveChangesAsync();


                return Ok(new
                {
                    Status = true,
                    Message = "Business Updated Successfully"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusinessExists(id))
                {
                    return NotFound(new
                    {
                        Status = false,
                        Message = "Business Not Found"
                    });
                }

                throw;
            }
        }


        // DELETE: api/BusinessMaster/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            var business = await _context.BusinessMaster
                .FindAsync(id);


            if (business == null)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Business Not Found"
                });
            }


            _context.BusinessMaster.Remove(business);

            await _context.SaveChangesAsync();


            return Ok(new
            {
                Status = true,
                Message = "Business Deleted Successfully"
            });
        }


        private bool BusinessExists(int id)
        {
            return _context.BusinessMaster
                .Any(e => e.BusinessId == id);
        }
    }
}