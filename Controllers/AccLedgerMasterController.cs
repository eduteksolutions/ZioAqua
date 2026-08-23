using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccLedgerMasterController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AccLedgerMasterController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/AccLedgerMaster?userId=1
        [HttpGet]
        public IActionResult Get(int userId)
        {
            var list = new List<object>();

            using var con = _db.CreateConnection();
            con.Open();

            string sql = @"
                SELECT 
                    l.ACode,
                    l.AccName,
                    g.AccGroupName,
                    l.Add1,
                    l.Add2,
                    l.PhoneNo,
                    l.Email,
                    l.OpenBal,
                    l.OpenBalType,
                    l.PCode,
                    l.BusinessId,
                    l.LoginName
                FROM AccLedgerMaster l
                INNER JOIN AccMaster g
                    ON l.PCode = g.Code
                WHERE l.BusinessId = @BusinessId
                ORDER BY l.AccName";

            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@BusinessId", userId);

            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new
                {
                    ACode = dr["ACode"],
                    AccName = dr["AccName"],
                    AccGroupName = dr["AccGroupName"],
                    Add1 = dr["Add1"],
                    Add2 = dr["Add2"],
                    PhoneNo = dr["PhoneNo"],
                    Email = dr["Email"],
                    OpenBal = dr["OpenBal"],
                    OpenBalType = dr["OpenBalType"],
                    PCode = dr["PCode"],
                    BusinessId = dr["BusinessId"],
                    LoginName = dr["LoginName"]
                });
            }

            return Ok(list);
        }


        // POST: api/AccLedgerMaster
        [HttpPost]
        public IActionResult Post([FromBody] AccLedgerMaster model)
        {
            using var con = _db.CreateConnection();
            con.Open();

            string sql = @"
                INSERT INTO AccLedgerMaster
                (
                    AccName,
                    Add1,
                    Add2,
                    GST,
                    Email,
                    PhoneNo,
                    OpenBalType,
                    OpenBal,
                    PCode,
                    BusinessId,
                    LoginName
                )
                VALUES
                (
                    @AccName,
                    @Add1,
                    @Add2,
                    @GST,
                    @Email,
                    @PhoneNo,
                    @OpenBalType,
                    @OpenBal,
                    @PCode,
                    @BusinessId,
                    @LoginName
                )";

            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@AccName", model.AccName ?? "");
            cmd.Parameters.AddWithValue("@Add1", model.Add1 ?? "");
            cmd.Parameters.AddWithValue("@Add2", model.Add2 ?? "");
            cmd.Parameters.AddWithValue("@GST", model.GST ?? "");
            cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@PhoneNo", model.PhoneNo ?? "");
            cmd.Parameters.AddWithValue("@OpenBalType", model.OpenBalType);
            cmd.Parameters.AddWithValue("@OpenBal", model.OpenBal);
            cmd.Parameters.AddWithValue("@PCode", model.PCode);
            cmd.Parameters.AddWithValue("@BusinessId", model.BusinessId);
            cmd.Parameters.AddWithValue("@LoginName", model.LoginName ?? "");

            cmd.ExecuteNonQuery();

            return Ok(new
            {
                Status = true,
                Message = "Ledger Saved Successfully"
            });
        }


        // PUT: api/AccLedgerMaster/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AccLedgerMaster model)
        {
            using var con = _db.CreateConnection();
            con.Open();

            string sql = @"
                UPDATE AccLedgerMaster
                SET
                    AccName = @AccName,
                    Add1 = @Add1,
                    Add2 = @Add2,
                    GST = @GST,
                    Email = @Email,
                    PhoneNo = @PhoneNo,
                    OpenBal = @OpenBal,
                    OpenBalType = @OpenBalType,
                    PCode = @PCode
                WHERE
                    ACode = @ACode
                    AND BusinessId = @BusinessId";

            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@ACode", id);
            cmd.Parameters.AddWithValue("@AccName", model.AccName ?? "");
            cmd.Parameters.AddWithValue("@Add1", model.Add1 ?? "");
            cmd.Parameters.AddWithValue("@Add2", model.Add2 ?? "");
            cmd.Parameters.AddWithValue("@GST", model.GST ?? "");
            cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@PhoneNo", model.PhoneNo ?? "");
            cmd.Parameters.AddWithValue("@OpenBal", model.OpenBal);
            cmd.Parameters.AddWithValue("@OpenBalType", model.OpenBalType);
            cmd.Parameters.AddWithValue("@PCode", model.PCode);
            cmd.Parameters.AddWithValue("@BusinessId", model.BusinessId);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Ledger not found for this Business"
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Ledger Updated Successfully"
            });
        }


        // DELETE: api/AccLedgerMaster/5?businessId=1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, int businessId)
        {
            using var con = _db.CreateConnection();
            con.Open();

            string sql = @"
                DELETE FROM AccLedgerMaster
                WHERE
                    ACode = @ACode
                    AND BusinessId = @BusinessId";

            using var cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@ACode", id);
            cmd.Parameters.AddWithValue("@BusinessId", businessId);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Ledger not found for this Business"
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Ledger Deleted Successfully"
            });
        }
    }
}