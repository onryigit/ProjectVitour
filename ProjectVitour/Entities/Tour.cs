using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class Tour
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourID { get; set; }

        public string Title { get; set; }
        public string Title_EN { get; set; }
        public string Title_DE { get; set; }
        public string Description { get; set; }
        public string Description_EN { get; set; }
        public string Description_DE { get; set; }
        public string CoverImageUrl { get; set; }
        public string Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public string Location { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string DestinationID { get; set; }
        public string MapLocationImageUrl { get; set; }
    }
}