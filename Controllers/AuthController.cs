
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BCrypt;

namespace RaceDayAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class RegisterRequest
        {
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
            public string UserRole { get; set; } = "";
        }

        public class LoginRequest
        {
            public string Email { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Please complete all required fields.");

            if (request.UserRole != "Organiser" &&
                request.UserRole != "Participant")
                return BadRequest("Role must be Organiser or Participant.");

            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand check = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email = @Email", connection);
            check.Parameters.AddWithValue("@Email", request.Email);

            if ((int)check.ExecuteScalar()! > 0)
                return Conflict("An account with this email already exists.");

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            using SqlCommand command = new SqlCommand(
                @"INSERT INTO Users (FirstName, LastName, Email, PasswordHash, UserRole)
                  VALUES (@FirstName, @LastName, @Email, @PasswordHash, @UserRole)", connection);

            command.Parameters.AddWithValue("@FirstName", request.FirstName);
            command.Parameters.AddWithValue("@LastName", request.LastName);
            command.Parameters.AddWithValue("@Email", request.Email);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@UserRole", request.UserRole);
            command.ExecuteNonQuery();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            string connectionString =
                _configuration.GetConnectionString("RaceDayConnection")!;

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand command = new SqlCommand(
                "SELECT UserID, PasswordHash, UserRole FROM Users WHERE Email = @Email",
                connection);

            command.Parameters.AddWithValue("@Email", request.Email);

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
                return Unauthorized("Invalid email or password.");

            int userId = reader.GetInt32(0);
            string passwordHash = reader.GetString(1);
            string userRole = reader.GetString(2);

            if (!BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
                return Unauthorized("Invalid email or password.");

            reader.Close();

            HttpContext.Session.SetInt32("UserID", userId);
            HttpContext.Session.SetString("UserRole", userRole);

            return Ok(new
            {
                Message = "Login successful.",
                UserID = userId,
                UserRole = userRole
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logged out successfully.");
        }
    }
}
