using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.ReviewServices;

namespace ProjectVitour.Controllers
{
    public class AdminReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public AdminReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> ReviewList()
        {
            var values = await _reviewService.GetAllReviewAsync();
            return View(values);
        }

        public async Task<IActionResult> DeleteReview(string id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return RedirectToAction("ReviewList");
        }

        public async Task<IActionResult> ApproveReview(string id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review != null)
            {
                // Status değerini true (Onaylı/Yayında) yapıyoruz
                var updateDto = new ProjectVitour.Dtos.ReviewDtos.UpdateReviewDto
                {
                    ReviewId = review.ReviewId,
                    NameSurname = review.NameSurname,
                    Detail = review.Detail,
                    GuideRating = review.GuideRating,
                    AccommodationRating = review.AccommodationRating,
                    TransportRating = review.TransportRating,
                    ComfortRating = review.ComfortRating,
                    TourId = review.TourId,
                    ReviewDate = review.ReviewDate,
                    Status = true
                };
                
                await _reviewService.UpdateReviewAsync(updateDto);
            }
            return RedirectToAction("ReviewList");
        }
    }
}