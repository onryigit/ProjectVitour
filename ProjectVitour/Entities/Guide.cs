using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class Guide
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GuideID { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; } // Örn: Kıdemli Tur Rehberi
        public string ImageUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string TwitterUrl { get; set; }
        public bool Status { get; set; }
    }
}