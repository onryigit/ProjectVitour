using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class TourPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourPlanID { get; set; }

        // İlişkiyi sağlayacak Foreign Key mantığındaki alanımız
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourID { get; set; }

        public int DayNumber { get; set; } // Örn: 1, 2, 3...
        public string Title { get; set; } // Örn: "Day 1: Arrive in Zürich"
        public string Description { get; set; } // O günün detaylı açıklaması
    }
}