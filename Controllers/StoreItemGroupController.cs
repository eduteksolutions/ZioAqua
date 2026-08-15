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
            var data = await _db.StoreItemGroup
                .Where(x => x.BusinessId == userid)
                .OrderBy(x => x.IGrpName)
                .ToListAsync();

            return Ok(data);
        }


        // POST: api/StoreItemGroup
        [HttpPost]
        public async Task<IActionResult> Post(StoreItemGroup model)
        {
            model.LUserDt = DateTime.Now;

            _db.StoreItemGroup.Add(model);

            await _db.SaveChangesAsync();


            return Ok(new
            {
                Code = 200,
                Status = true,
                Message = "Store Group Added Successfully"
            });
        }


        // PUT: api/StoreItemGroup/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            int id,
            StoreItemGroup model)
        {
            var group = await _db.StoreItemGroup
                .FirstOrDefaultAsync(x =>
                    x.IGrpCd == id &&
                    x.BusinessId == model.BusinessId);


            if (group == null)
                return NotFound();


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
                Message = "Store Group Updated Successfully"
            });
        }


        // DELETE: api/StoreItemGroup/1?userid=1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            int id,
            int userid)
        {
            var group = await _db.StoreItemGroup
                .FirstOrDefaultAsync(x =>
                    x.IGrpCd == id &&
                    x.BusinessId == userid);


            if (group == null)
                return NotFound();


            _db.StoreItemGroup.Remove(group);

            await _db.SaveChangesAsync();


            return Ok(new
            {
                Code = 200,
                Status = true,
                Message = "Store Group Deleted Successfully"
            });
        }
    }
}