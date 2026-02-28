using ProjectVitour.Dtos.TourImageDtos;

namespace ProjectVitour.Dtos.TourDtos
{
    public class GetTourByIdDto
    {
        public string TourID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public string Location { get; set; }
        public string MapLocationImageUrl { get; set; }

        // Turun gün gün planlarını tutacak liste
        public List<TourPlanDto> TourPlans { get; set; }
        public List<ProjectVitour.Dtos.ReviewDtos.ResultReviewByTourIdDto> Reviews { get; set; }
        public List<ResultTourImageDto> TourImages { get; set; }
    }
}
