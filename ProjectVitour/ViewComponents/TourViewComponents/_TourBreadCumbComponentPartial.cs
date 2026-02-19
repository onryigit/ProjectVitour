using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourBreadCumbComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}
