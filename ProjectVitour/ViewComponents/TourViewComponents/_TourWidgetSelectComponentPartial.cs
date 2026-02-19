using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourWidgetSelectComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
