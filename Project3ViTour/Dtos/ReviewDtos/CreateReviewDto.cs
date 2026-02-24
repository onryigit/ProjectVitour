namespace Project3ViTour.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public string TourId { get; set; }
    }
}
