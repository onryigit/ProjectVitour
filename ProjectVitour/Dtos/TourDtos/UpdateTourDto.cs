namespace ProjectVitour.Dtos.TourDtos
{
    public class UpdateTourDto
    {
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
        public string DestinationID { get; set; }
        public string CategoryID { get; set; }
        public string MapLocationImageUrl { get; set; }
    }
}
