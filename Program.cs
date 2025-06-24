using StackExchange.Redis;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthenticationServer {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();

			// Execute the command below to activate redis:
			// docker run -p 6379:6379 redis
			string? redisConfig = Environment.GetEnvironmentVariable("REDIS_CONFIGURATION");
			string? mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
			string? mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME");

			// For testing purposes
			redisConfig = "localhost";
			mongoConnectionString = "mongodb://localhost:27017";
			mongoDatabaseName = "AuthDb";

			builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfig));
			builder.Services.AddHttpClient();

			builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(mongoConnectionString));

			builder.Services.AddSingleton<UserRepository>(sp => new UserRepository(mongoDatabaseName, sp.GetService<IMongoClient>()));

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll", policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseCors("AllowAll");

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
