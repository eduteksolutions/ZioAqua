using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccGroupController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AccGroupController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string masterType)
        {
            var dt = new DataTable();

            using SqlConnection con = _db.CreateConnection();
            con.Open();

            string[] values = masterType.Split(',');

            var parameters = new List<string>();

            using SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            for (int i = 0; i < values.Length; i++)
            {
                string parameterName = "@MasterType" + i;

                parameters.Add(parameterName);

                cmd.Parameters.Add(
                    parameterName,
                    System.Data.SqlDbType.Int
                ).Value = int.Parse(values[i].Trim());
            }

            string sql = $@"
        SELECT *
        FROM AccGroupMaster
        WHERE MasterType IN ({string.Join(",", parameters)})
        ORDER BY AccGroupName";

            cmd.CommandText = sql;

            using SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            var result = dt.AsEnumerable()
                .Select(row => dt.Columns
                    .Cast<DataColumn>()
                    .ToDictionary(
                        column => column.ColumnName,
                        column => row[column] == DBNull.Value
                            ? null
                            : row[column]
                    ))
                .ToList();

            return Ok(result);
        }  // POST: api/AccGroup
        [HttpPost]
        public IActionResult Post([FromBody] AccGroupMaster model)
        {
            using SqlConnection con = _db.CreateConnection();
            con.Open();

            string sql = @"
                INSERT INTO AccGroupMaster
                (
                    AccGroupName,
                    MasterType,
                    BusinessId
                )
                VALUES
                (
                    @AccGroupName,
                    @MasterType,
                    @BusinessId
                )";

            using SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue(
                "@AccGroupName",
                model.AccGroupName ?? "");

            cmd.Parameters.AddWithValue(
                "@MasterType",
                model.MasterType);

            cmd.Parameters.AddWithValue(
                "@BusinessId",
                model.BusinessId);

            cmd.ExecuteNonQuery();

            return Ok(new
            {
                Status = true,
                Message = "Group Added Successfully"
            });
        }


        // PUT: api/AccGroup/5
        [HttpPut("{id}")]
        public IActionResult Put(
            int id,
            [FromBody] AccGroupMaster model)
        {
            using SqlConnection con = _db.CreateConnection();
            con.Open();

            string sql = @"
                UPDATE AccGroupMaster
                SET
                    AccGroupName = @AccGroupName,
                    MasterType = @MasterType
                WHERE
                    Code = @Code
                    AND BusinessId = @BusinessId";

            using SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@Code", id);

            cmd.Parameters.AddWithValue(
                "@AccGroupName",
                model.AccGroupName ?? "");

            cmd.Parameters.AddWithValue(
                "@MasterType",
                model.MasterType );

            cmd.Parameters.AddWithValue(
                "@BusinessId",
                model.BusinessId);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Group not found"
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Updated Successfully"
            });
        }


        // DELETE: api/AccGroup/5?businessId=1
        [HttpDelete("{id}")]
        public IActionResult Delete(
            int id,
            int businessId)
        {
            using SqlConnection con = _db.CreateConnection();
            con.Open();

            string sql = @"
                DELETE FROM AccGroupMaster
                WHERE
                    Code = @Code
                    AND BusinessId = @BusinessId";

            using SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@Code", id);
            cmd.Parameters.AddWithValue("@BusinessId", businessId);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                return NotFound(new
                {
                    Status = false,
                    Message = "Group not found"
                });
            }

            return Ok(new
            {
                Status = true,
                Message = "Deleted Successfully"
            });
        }
    }
}