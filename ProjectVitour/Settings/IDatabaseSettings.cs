namespace ProjectVitour.Settings
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string TourCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ReviewCollectionName { get; set; }
        public string TourPlanCollectionName { get; set; }
        public string ReservationCollectionName { get; set; }
        public string TourImageCollectionName { get; set; }
        public string ContactMessageCollectionName { get; set; }
        public string DestinationCollectionName { get; set; }
        public string GuideCollectionName { get; set; }
    }
}
