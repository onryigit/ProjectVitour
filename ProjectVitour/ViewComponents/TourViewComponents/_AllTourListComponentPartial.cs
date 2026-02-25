using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.TourServices;

namespace ProjectVitour.ViewComponents.TourViewComponents
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
            var values = await _tourService.GetAllToursAsync();
            return View(values);
        }
    }
}
