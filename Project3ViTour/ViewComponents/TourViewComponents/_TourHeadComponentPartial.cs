using Microsoft.AspNetCore.Mvc;

namespace Project3ViTour.ViewComponents.TourViewComponents
{
    public class _TourHeadComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
