using Microsoft.AspNetCore.Mvc;

namespace Project3ViTour.ViewComponents.TourViewComponents
{
    public class _TourScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
