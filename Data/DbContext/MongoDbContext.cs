using Film_Ai.Models.Entities;
using MongoDB.Driver;

namespace Film_Ai.Data.DbContext
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient, string databaseName)
        {
            _database = mongoClient.GetDatabase(databaseName);
        }
        public IMongoCollection<Movie> Movie => _database.GetCollection<Movie>("Movies");
    }
}
