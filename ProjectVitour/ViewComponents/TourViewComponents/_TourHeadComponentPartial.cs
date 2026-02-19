using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourHeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
