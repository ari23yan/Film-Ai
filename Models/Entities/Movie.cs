using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Film_Ai.Models.Entities
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? ReleaseYear { get; set; }
        public string? Title { get; set; }
        public string? Poster { get; set; }
        public string? Description { get; set; }
        public string? Description_Fa { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsSeries { get; set; } = false;
    }
}
