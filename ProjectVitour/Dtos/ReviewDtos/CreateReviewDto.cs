namespace ProjectVitour.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public string TourId { get; set; }
        public int GuideRating { get; set; }         // Rehber Puanı
        public int AccommodationRating { get; set; } // Konaklama Puanı
        public int TransportRating { get; set; }     // Ulaşım Puanı
        public int ComfortRating { get; set; }       // Konfor Puanı

    }
}
