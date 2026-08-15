using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using zioAqua.model;

[ApiController]
[Route("api/[controller]")]
public class AccGroupController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AccGroupController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var dt = new DataTable();

        using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        con.Open();

        SqlDataAdapter da = new SqlDataAdapter(
            "SELECT * FROM AccGroupMaster ORDER BY AccGroupName", con);

        da.Fill(dt);

        return Ok(dt);
    }

    [HttpPost]
    public IActionResult Post(AccGroupMaster model)
    {
        using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        con.Open();

        SqlCommand cmd = new SqlCommand(
            @"INSERT INTO AccGroupMaster(AccGroupName,MasterType)
              VALUES(@AccGroupName,@MasterType)", con);

        cmd.Parameters.AddWithValue("@AccGroupName", model.AccGroupName);
        cmd.Parameters.AddWithValue("@MasterType", model.MasterType);

        cmd.ExecuteNonQuery();

        return Ok(new
        {
            Status = true,
            Message = "Group Added Successfully"
        });
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, AccGroupMaster model)
    {
        using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        con.Open();

        SqlCommand cmd = new SqlCommand(
            @"UPDATE AccGroupMaster
              SET AccGroupName=@AccGroupName,
                  MasterType=@MasterType
              WHERE Code=@Code", con);

        cmd.Parameters.AddWithValue("@Code", id);
        cmd.Parameters.AddWithValue("@AccGroupName", model.AccGroupName);
        cmd.Parameters.AddWithValue("@MasterType", model.MasterType);

        cmd.ExecuteNonQuery();

        return Ok(new
        {
            Status = true,
            Message = "Updated Successfully"
        });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        con.Open();

        SqlCommand cmd = new SqlCommand(
            "DELETE FROM AccGroupMaster WHERE Code=@Code", con);

        cmd.Parameters.AddWithValue("@Code", id);

        cmd.ExecuteNonQuery();

        return Ok(new
        {
            Status = true,
            Message = "Deleted Successfully"
        });
    }
}