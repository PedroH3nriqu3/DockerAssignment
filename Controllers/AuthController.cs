using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AuthenticationServer.Controllers {
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase {

		private IDatabase redis;

		public AuthController(IConnectionMultiplexer muxer) {
			redis = muxer.GetDatabase();
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
		public ActionResult Register([FromBody] User user) {
			// Add user to database

			return Ok();
		}
	}
}
