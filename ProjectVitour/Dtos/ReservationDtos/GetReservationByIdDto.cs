namespace ProjectVitour.Dtos.ReservationDtos
{
    public class GetReservationByIdDto
    {
        public string ReservationID { get; set; }
        public string TourID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int PersonCount { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool Status { get; set; }
    }
}