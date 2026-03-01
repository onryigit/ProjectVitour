using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.ReservationServices;
using ProjectVitour.Services.TourServices;
using ProjectVitour.Services.ContactMessageServices;

namespace ProjectVitour.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ITourService _tourService;
        private readonly IContactMessageService _messageService;

        public AdminDashboardController(IReservationService reservationService, ITourService tourService, IContactMessageService messageService)
        {
            _reservationService = reservationService;
            _tourService = tourService;
            _messageService = messageService;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllReservationAsync();
            var tours = await _tourService.GetAllToursAsync();
            var messages = await _messageService.GetAllMessagesAsync();
            ViewBag.TotalTours = tours.Count;
            ViewBag.TotalReservations = reservations.Count;
            ViewBag.PendingReservations = reservations.Count(x => !x.Status);
            ViewBag.UnreadMessages = messages.Count(x => !x.IsRead);
            decimal totalEarning = 0;
            foreach (var res in reservations)
            {
                var tourPrice = tours.FirstOrDefault(x => x.TourID == res.TourID)?.Price ?? 0;
                totalEarning += (tourPrice * res.PersonCount);
            }
            ViewBag.TotalEarning = totalEarning;
            var tourNames = new List<string>();
            var tourResCounts = new List<int>();
            var popularTours = reservations.GroupBy(r => r.TourID)
                                           .OrderByDescending(g => g.Count())
                                           .Take(5)
                                           .Select(g => new { TourID = g.Key, Count = g.Count() })
                                           .ToList();

            if (popularTours.Any())
            {
                foreach (var item in popularTours)
                {
                    var tourTitle = tours.FirstOrDefault(x => x.TourID == item.TourID)?.Title ?? "Bilinmeyen Tur";
                    tourNames.Add(tourTitle);
                    tourResCounts.Add(item.Count);
                }
            }
            else
            {
                foreach (var tour in tours.Take(5))
                {
                    tourNames.Add(tour.Title);
                    tourResCounts.Add(0);
                }
            }

            ViewBag.ChartLabels = tourNames;
            ViewBag.ChartData = tourResCounts;
            var last5Reservations = reservations.OrderByDescending(x => x.ReservationDate).Take(5).ToList();
            ViewBag.LastReservations = last5Reservations;
            ViewBag.Tours = tours;

            return View();
        }
    }
}