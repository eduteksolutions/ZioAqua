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
                var data = await _db.tblStoreItemMast
                    .Where(x => x.BusinessId == userid)
                    .OrderBy(x => x.ICodeNum)
                    .ToListAsync();

                return Ok(data);
            }



            // POST: api/StoreItemMaster
            [HttpPost]
            public async Task<IActionResult> Post(tblStoreItemMast model)
            {
                model.LUserDt = DateTime.Now;

                _db.tblStoreItemMast.Add(model);

                await _db.SaveChangesAsync();


                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Item Added Successfully"
                });
            }



            // PUT: api/StoreItemMaster/5
            [HttpPut("{id}")]
            public async Task<IActionResult> Put(
                int id,
                tblStoreItemMast model)
            {
                var item = await _db.tblStoreItemMast
                    .FirstOrDefaultAsync(x =>
                        x.ICodeNum == id &&
                        x.BusinessId == model.BusinessId);


                if (item == null)
                    return NotFound();



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
                    Message = "Item Updated Successfully"
                });
            }



            // DELETE: api/StoreItemMaster/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(
                int id,
                int userid)
            {
                var item = await _db.tblStoreItemMast
                    .FirstOrDefaultAsync(x =>
                        x.ICodeNum == id &&
                        x.BusinessId == userid);


                if (item == null)
                    return NotFound();


                _db.tblStoreItemMast.Remove(item);

                await _db.SaveChangesAsync();

            //9289375253

                return Ok(new
                {
                    Code = "200",
                    Status = true,
                    Message = "Item Deleted Successfully"
                });
            }
        }
    }

