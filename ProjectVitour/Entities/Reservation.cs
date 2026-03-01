using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectVitour.Entities
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReservationID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string TourID { get; set; }

        public string ReservationCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PersonCount { get; set; } // Kontenjan kontrolü için kullanılacak
        public DateTime ReservationDate { get; set; }
        public bool Status { get; set; } // Rezervasyon onay durumu
    }
}
