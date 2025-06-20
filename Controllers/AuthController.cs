using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AuthenticationServer.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase {

		private IDatabase redis;
		private readonly UserRepository _userRepository;
		public AuthController(IConnectionMultiplexer muxer, UserRepository userRepository) {
			redis = muxer.GetDatabase();
			_userRepository = userRepository;
		}

		[Route("login")]
		[HttpPost]
		public async Task<ActionResult> Login([FromBody] User user) {
			string? storedPassword = await redis.StringGetAsync(user.Username);
			if (String.IsNullOrEmpty(storedPassword)) {
				// Implement login functionality with MongoDB.
				// Fetch user password from database and match it
				// against the provided password.
				// If they're equal, store the password inside redis,
				// else return status 401.
				// Save to redis only if login is accepted.

				storedPassword = user.Password;
				await redis.StringSetAsync(user.Username, storedPassword);
				await redis.KeyExpireAsync(user.Username, TimeSpan.FromSeconds(60));

			} else if (user.Password == storedPassword) {
				return Ok();
			}

			return Unauthorized();
		}

		[Route("register")]
		[HttpPost]
		public async Task<IActionResult> Register(User user){
    		var existingUser = await _userRepository.GetByUsernameAsync(user.Username);
    		if (existingUser != null)
        		return BadRequest("Usuário já existe");

   		await _userRepository.CreateAsync(user);
        return Ok("Usuário registrado com sucesso");
}
	}
}
