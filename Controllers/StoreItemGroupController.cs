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

        [HttpGet]
        public async Task<IActionResult> Get(int userid)
        {
            try
            {
                // Example using an Inner Join with another table (e.g., tblStoreRackMaster or Users)
                // Replace 'tblStoreRackMaster' and join conditions with your actual second table if needed
                var data = await _db.tblStoreItemGrpMast
                    .Where(x => x.BusinessId == userid)
                    .Join(
                        _db.tblStoreItemMast, // The table you want to inner join with
                        group => group.IGrpCd,     // Outer key
                        item => item.IGrpCd,       // Inner key
                        (group, item) => new       // Projection/Select fields
                        {
                            group.IGrpCd,
                            group.IGrpName,
                            group.IGrpDescr,
                            group.BusinessId,
                            group.IRackCd,
                            group.LoginName,
                          
                       
                           
                            // Add extra fields from the joined table if required
                            ItemCodeNum = item.ICodeNum,
                            ItemName = item.IName
                        }
                    )
                    .OrderBy(x => x.IGrpName)
                    .ToListAsync();

                if (data == null || data.Count == 0)
                {
                    return NotFound(new
                    {
                        code = "404",
                        status = false,
                        message = "Record Not Found"
                    });
                }

                return Ok(new
                {
                    code = "200",
                    status = true,
                    message = "Store Groups Retrieved Successfully",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = "500",
                    status = false,
                    message = "Error while retrieving Store Groups",
                    error = ex.Message
                });
            }
        }

        // =========================================================
        // GET BY ID
        // GET: api/StoreItemGroup/1?userid=1
        // =========================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, int userid)
        {
            try
            {
                var data = await _db.tblStoreItemGrpMast
                    .FirstOrDefaultAsync(x =>
                        x.IGrpCd == id &&
                        x.BusinessId == userid);

                if (data == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        status = false,
                        message = "Store Group not found"
                    });
                }

                return Ok(new
                {
                    code = 200,
                    status = true,
                    message = "Store Group Retrieved Successfully",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    status = false,
                    message = "Error while retrieving Store Group",
                    error = ex.Message
                });
            }
        }

        // =========================================================
        // POST
        // POST: api/StoreItemGroup
        // =========================================================
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] tblStoreItemGrpMast model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Invalid Store Group data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IGrpName))
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Store Group Name is required"
                    });
                }

                if (model.BusinessId <= 0)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "BusinessId is required"
                    });
                }

                // Check duplicate group name
                bool exists = await _db.tblStoreItemGrpMast
                    .AnyAsync(x =>
                        x.IGrpName == model.IGrpName &&
                        x.BusinessId == model.BusinessId);

                if (exists)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Store Group already exists"
                    });
                }

                // Set current date/time
                model.LUserDt = DateTime.Now;

                // Add record
                _db.tblStoreItemGrpMast.Add(model);

                // Save
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    status = true,
                    message = "Store Group Added Successfully",
                    data = model
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    status = false,
                    message = "Error while adding Store Group",
                    error = ex.Message
                });
            }
        }

        // =========================================================
        // PUT
        // PUT: api/StoreItemGroup/1
        // =========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] tblStoreItemGrpMast model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Invalid Store Group data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IGrpName))
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Store Group Name is required"
                    });
                }

                if (model.BusinessId <= 0)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "BusinessId is required"
                    });
                }

                var group = await _db.tblStoreItemGrpMast
                    .FirstOrDefaultAsync(x =>
                        x.IGrpCd == id &&
                        x.BusinessId == model.BusinessId);

                if (group == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        status = false,
                        message = "Store Group not found"
                    });
                }

                // Check duplicate name
                bool duplicate = await _db.tblStoreItemGrpMast
                    .AnyAsync(x =>
                        x.IGrpName == model.IGrpName &&
                        x.BusinessId == model.BusinessId &&
                        x.IGrpCd != id);

                if (duplicate)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        status = false,
                        message = "Another Store Group with the same name already exists"
                    });
                }

                // Update fields
                group.IGrpName = model.IGrpName;
                group.IRackCd = model.IRackCd;
                group.IGrpDescr = model.IGrpDescr;
                group.LoginName = model.LoginName;
                group.LUserDt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    status = true,
                    message = "Store Group Updated Successfully",
                    data = group
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    status = false,
                    message = "Error while updating Store Group",
                    error = ex.Message
                });
            }
        }

        // =========================================================
        // DELETE
        // DELETE: api/StoreItemGroup/1?userid=1
        // =========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            int userid)
        {
            try
            {
                var group = await _db.tblStoreItemGrpMast
                    .FirstOrDefaultAsync(x =>
                        x.IGrpCd == id &&
                        x.BusinessId == userid);

                if (group == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        status = false,
                        message = "Store Group not found"
                    });
                }

                _db.tblStoreItemGrpMast.Remove(group);

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    status = true,
                    message = "Store Group Deleted Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    status = false,
                    message = "Error while deleting Store Group",
                    error = ex.Message
                });
            }
        }
    }
}