using Microsoft.AspNetCore.Mvc;
using Project3ViTour.Services.TourService;

namespace Project3ViTour.ViewComponents.TourViewComponents
{
    public class _AllTourListComponentPartial: ViewComponent
    {
        private readonly ITourService _tourService;
        public _AllTourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tours =await _tourService.GetAllToursAsync();
            return View(tours);
        }
    }
}
