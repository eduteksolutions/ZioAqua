using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreItemGroupController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StoreItemGroupController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/StoreItemGroup?userid=1
        [HttpGet]
        public async Task<IActionResult> Get(int userid)
        {
            try
            {
                var data = await _db.tblStoreGroupMaster
                    .Where(x => x.BusinessId == userid)
                    .OrderBy(x => x.IGrpName)
                    .ToListAsync();

                return Ok(new
                {
                    Code = 200,
                    Status = true,
                    Message = "Store Groups Retrieved Successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = 500,
                    Status = false,
                    Message = "Error while retrieving Store Groups",
                    Error = ex.Message
                });
            }
        }

        // POST: api/StoreItemGroup
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] tblStoreGroupMaster model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Code = 400,
                        Status = false,
                        Message = "Invalid Store Group data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IGrpName))
                {
                    return BadRequest(new
                    {
                        Code = 400,
                        Status = false,
                        Message = "Store Group Name is required"
                    });
                }

                model.LUserDt = DateTime.Now;

                _db.tblStoreGroupMaster.Add(model);


                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = 200,
                    Status = true,
                    Message = "Store Group Added Successfully",
                    Data = model
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = 500,
                    Status = false,
                    Message = "Error while adding Store Group",
                    Error = ex.Message
                });
            }
        }

        // PUT: api/StoreItemGroup/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] tblStoreGroupMaster model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Code = 400,
                        Status = false,
                        Message = "Invalid Store Group data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IGrpName))
                {
                    return BadRequest(new
                    {
                        Code = 400,
                        Status = false,
                        Message = "Store Group Name is required"
                    });
                }

                var group = await _db.tblStoreGroupMaster
                    .FirstOrDefaultAsync(x =>
                        x.IGrpCd == id &&
                        x.BusinessId == model.BusinessId);

                if (group == null)
                {
                    return NotFound(new
                    {
                        Code = 404,
                        Status = false,
                        Message = "Store Group not found"
                    });
                }

                group.IGrpName = model.IGrpName;
                group.IRackCd = model.IRackCd;
                group.IGrpDescr = model.IGrpDescr;
                group.LoginName = model.LoginName;
                group.LUserDt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = 200,
                    Status = true,
                    Message = "Store Group Updated Successfully",
                    Data = group
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = 500,
                    Status = false,
                    Message = "Error while updating Store Group",
                    Error = ex.Message
                });
            }
        }

        // DELETE: api/StoreItemGroup/1?userid=1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            int userid)
        {
            try
            {
                var group = await _db.tblStoreGroupMaster
                    .FirstOrDefaultAsync(x =>
                        x.IGrpCd == id &&
                        x.BusinessId == userid);

                if (group == null)
                {
                    return NotFound(new
                    {
                        Code = 404,
                        Status = false,
                        Message = "Store Group not found"
                    });
                }

                _db.tblStoreGroupMaster.Remove(group);

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = 200,
                    Status = true,
                    Message = "Store Group Deleted Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = 500,
                    Status = false,
                    Message = "Error while deleting Store Group",
                    Error = ex.Message
                });
            }
        }
    }
}