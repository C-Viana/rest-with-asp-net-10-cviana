using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Services;

namespace rest_with_asp_net_10_cviana.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly ILoginServices _services;
        private readonly IUsersAuthServices _usersAuthServices;

        public AuthController(ILogger<PersonController> logger, ILoginServices services, IUsersAuthServices usersAuthServices)
        {
            _logger = logger;
            _services = services;
            _usersAuthServices = usersAuthServices;
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignIn([FromBody] UsersDTO user)
        {
            _logger.LogInformation("Attempting to sign in user {username}", user.Username);
            if(user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                _logger.LogError("Sign in failed: missing username and/or password");
                return BadRequest("Username and/or password are either missing or empty");
            }
            var token = _services.ValidateCredentials(user);
            if(token == null)
            {
                _logger.LogWarning("Sign in failed: invalid credentials for user {username}", user.Username);
                return Unauthorized();
            }
            _logger.LogInformation("{username} signed in successfully", user.Username);
            return Ok(token);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RefreshToken([FromBody] TokenDTO token)
        {
            _logger.LogInformation("Attempting to refresh token");
            if (token == null || string.IsNullOrWhiteSpace(token.RefreshToken))
            {
                _logger.LogError("Refresh token failed: missing token value");
                return BadRequest("Token information is either missing or empty");
            }
            var refreshToken = _services.ValidateCredentials(token);
            if (refreshToken == null)
            {
                _logger.LogWarning("Refresh token failed: the given refresh token is either invalid or has expired");
                return Unauthorized();
            }
            _logger.LogInformation("Token has been renewed successfully");
            return Ok(refreshToken);
        }

        [HttpPost("revoke")]
        [Authorize]
        [ProducesResponseType(typeof(TokenDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RevokeToken()
        {
            _logger.LogInformation("Attempting to revoke token");
            var username = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogError("Revoke token failed: missing user data");
                return BadRequest("Invalid request: missing user data");
            }
            var result = _services.RevokeToken(username);
            if (!result)
            {
                _logger.LogWarning("Revoke token failed: user {username} not found", username);
                return BadRequest("Invalid request");
            }
            _logger.LogInformation("Token has been renewed successfully");
            return NoContent();
        }

        [HttpPost("create")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AccountCredentialsDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Create([FromBody] AccountCredentialsDTO user)
        {
            _logger.LogInformation("Attempting to create user {username}", user.Username);
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Fullname))
            {
                _logger.LogError("Creation failed: missing username, full name and/or password");
                return BadRequest("Username, full name and/or password are either missing or empty");
            }
            var createdUser = _services.Create(user);
            if (createdUser == null)
            {
                _logger.LogWarning("Creation failed");
                return Problem("User creation runned into an error. Check on logs for more information");
            }
            _logger.LogInformation("User {username} created successfully", user.Username);
            return CreatedAtAction(null, null, createdUser);
        }
    }
}
