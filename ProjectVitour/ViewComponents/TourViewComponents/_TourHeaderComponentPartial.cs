using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourHeaderComponentPartial: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); 
        }
    }
}
