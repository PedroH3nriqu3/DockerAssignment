using MongoDB.Bson.Serialization.Attributes;

namespace AuthenticationServer {
	public class User {
		[BsonId]
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
