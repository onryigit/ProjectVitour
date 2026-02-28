using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class TourImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourImageID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourID { get; set; }

        public string ImageUrl { get; set; }
    }
}