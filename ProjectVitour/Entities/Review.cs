using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewId { get; set; }

        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public int GuideRating { get; set; }         // Rehber
        public int AccommodationRating { get; set; } // Konaklama
        public int TransportRating { get; set; }     // Ulaşım
        public int ComfortRating { get; set; }       // Konfor

        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }
    }
}