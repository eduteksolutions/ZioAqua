using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StoreItemMasterController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StoreItemMasterController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/StoreItemMaster?userid=1
        [HttpGet]
        public async Task<IActionResult> Get(int userid)
        {
            try
            {
                var data = await _db.tblStoreItemMast
                    .Where(x => x.BusinessId == userid)
                    .OrderBy(x => x.ICodeNum)
                    .ToListAsync();

                if (data == null || data.Count == 0)
                {
                    return NotFound(new
                    {
                        Code = "404",
                        Status = false,
                        Message = "Record Not Found"
                    });
                }

                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Records Found",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = "500",
                    Status = false,
                    Message = "Error while retrieving Items",
                    Error = ex.Message
                });
            }
        }

        // POST: api/StoreItemMaster
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] tblStoreItemMast model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Code = "400",
                        Status = false,
                        Message = "Invalid Item data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IName))
                {
                    return BadRequest(new
                    {
                        Code = "400",
                        Status = false,
                        Message = "Item Name is required"
                    });
                }

                // Get next Item Code for this Business
                int maxCode = await _db.tblStoreItemMast
                    .Where(x => x.BusinessId == model.BusinessId)
                    .Select(x => (int?)x.ICodeNum)
                    .MaxAsync() ?? 0;

                model.ICodeNum = maxCode + 1;

                model.LUserDt = DateTime.Now;

                _db.tblStoreItemMast.Add(model);

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Item Added Successfully",
                    Data = model
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = "500",
                    Status = false,
                    Message = "Error while adding Item",
                    Error = ex.Message
                });
            }
        }
        // PUT: api/StoreItemMaster/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            [FromBody] tblStoreItemMast model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new
                    {
                        Code = "400",
                        Status = false,
                        Message = "Invalid Item data"
                    });
                }

                if (string.IsNullOrWhiteSpace(model.IName))
                {
                    return BadRequest(new
                    {
                        Code = "400",
                        Status = false,
                        Message = "Item Name is required"
                    });
                }

                var item = await _db.tblStoreItemMast
                    .FirstOrDefaultAsync(x =>
                        x.ICodeNum == id &&
                        x.BusinessId == model.BusinessId);

                if (item == null)
                {
                    return NotFound(new
                    {
                        Code = "404",
                        Status = false,
                        Message = "Item not found"
                    });
                }

                item.ICodeStr = model.ICodeStr;
                item.IName = model.IName;
                item.IShortName = model.IShortName;
                item.IGrpCd = model.IGrpCd;
                item.IUom = model.IUom;
                item.ISaleTax = model.ISaleTax;
                item.IPrate = model.IPrate;
                item.IMrp = model.IMrp;
                item.IStock = model.IStock;
                item.IMargin = model.IMargin;
                item.Openingstock = model.Openingstock;
                item.Openingstatus = model.Openingstatus;
                item.LoginName = model.LoginName;
                item.LUserDt = DateTime.Now;

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Item Updated Successfully",
                    Data = item
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = "500",
                    Status = false,
                    Message = "Error while updating Item",
                    Error = ex.Message
                });
            }
        }

        // DELETE: api/StoreItemMaster/5?userid=1
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            int userid)
        {
            try
            {
                var item = await _db.tblStoreItemMast
                    .FirstOrDefaultAsync(x =>
                        x.ICodeNum == id &&
                        x.BusinessId == userid);

                if (item == null)
                {
                    return NotFound(new
                    {
                        Code = "404",
                        Status = false,
                        Message = "Item not found"
                    });
                }

                _db.tblStoreItemMast.Remove(item);

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Item Deleted Successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Code = "500",
                    Status = false,
                    Message = "Error while deleting Item",
                    Error = ex.Message
                });
            }
        }
    }
}