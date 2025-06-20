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
			builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
			builder.Services.AddHttpClient();
			
			builder.Services.Configure<MongoDbSettings>(
				builder.Configuration.GetSection("MongoDB"));

			builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
			{
				var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
				return new MongoClient(settings.ConnectionString);
			});

			builder.Services.AddSingleton<UserRepository>();


			var app = builder.Build();

			// Configure the HTTP request pipeline.

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
