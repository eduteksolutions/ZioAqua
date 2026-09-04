using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using zioAqua.Data;
using zioAqua.model;

namespace zioAqua.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CreateUserController(
            ApplicationDbContext context)
        {
            _context = context;
        }


        // POST: api/CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromBody] CreateUserRequest request)
        {
            try
            {
                // -----------------------------
                // VALIDATION
                // -----------------------------

                if (request.BusinessID <= 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "BusinessID is required."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    request.LoginName))
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "Login name is required."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    request.Password))
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "Password is required."
                    });
                }


                if (request.Password.Length < 8)
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message =
                            "Password must be at least 8 characters."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    request.UserName))
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "User name is required."
                    });
                }


                // -----------------------------
                // CHECK BUSINESS
                // -----------------------------

                var business = await _context.BusinessMaster
                    .FirstOrDefaultAsync(x =>
                        x.BusinessId
                        == request.BusinessID);

                if (business == null
                    )
                {
                    return NotFound(new ApiResponse
                    {
                        StatusCode = 404,
                        Success = false,
                        Message = "Business not found."
                    });
                }


                // -----------------------------
                // CHECK LOGIN NAME
                // -----------------------------

                bool loginExists =
                    await _context.UserMaster.AnyAsync(x =>
                        x.BusinessID == request.BusinessID &&
                        x.LoginName.ToLower()
                            == request.LoginName
                                .Trim()
                                .ToLower());

                if (loginExists)
                {
                    return Conflict(new ApiResponse
                    {
                        StatusCode = 409,
                        Success = false,
                        Message =
                            "Login name already exists for this business."
                    });
                }


                // -----------------------------
                // CHECK EMAIL
                // -----------------------------

                if (!string.IsNullOrWhiteSpace(
                    request.Email))
                {
                    bool emailExists =
                        await _context.UserMaster.AnyAsync(x =>
                            x.BusinessID == request.BusinessID &&
                            x.Email != null &&
                            x.Email.ToLower()
                                == request.Email
                                    .Trim()
                                    .ToLower());

                    if (emailExists)
                    {
                        return StatusCode(
                            StatusCodes.Status202Accepted,
                            new ApiResponse
                            {
                                StatusCode = 202,
                                Success = false,
                                Message =
                                    "Email already exists."
                            });
                    }
                }


                // -----------------------------
                // VALIDATE ROLE
                // -----------------------------

                string[] validRoles =
                {
                    "Admin",
                    "Sales",
                    "Delivery",
                    "Accounts",
                    "Staff"
                };

                if (!validRoles.Contains(
                    request.Role,
                    StringComparer.OrdinalIgnoreCase))
                {
                    return BadRequest(new ApiResponse
                    {
                        StatusCode = 400,
                        Success = false,
                        Message = "Invalid role."
                    });
                }


                // -----------------------------
                // HASH PASSWORD
                // -----------------------------

                string passwordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        request.Password);


                // -----------------------------
                // CREATE USER
                // -----------------------------

                var user = new UserMaster
                {
                    BusinessID =
                        request.BusinessID,

                    LoginName =
                        request.LoginName.Trim(),

                    PasswordHash =
                        passwordHash,

                    UserName =
                        request.UserName.Trim(),

                    Email =
                        string.IsNullOrWhiteSpace(
                            request.Email)
                            ? null
                            : request.Email.Trim(),

                    MobileNum =
                        string.IsNullOrWhiteSpace(
                            request.MobileNum)
                            ? null
                            : request.MobileNum.Trim(),

                    Role =
                        request.Role.Trim(),

                    IsActive =
                        request.IsActive,

                    CreatedDate =
                        DateTime.UtcNow
                };


                _context.UserMaster.Add(user);

                await _context.SaveChangesAsync();


                // -----------------------------
                // SUCCESS
                // -----------------------------

                return Ok(new ApiResponse
                {
                    StatusCode = 200,
                    Success = true,
                    Message =
                        "User created successfully.",
                    UserID =
                        user.UserID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ApiResponse
                    {
                        StatusCode = 500,
                        Success = false,
                        Message =
                            "Error creating user: "
                            + ex.Message
                    });
            }
        }
    }
}