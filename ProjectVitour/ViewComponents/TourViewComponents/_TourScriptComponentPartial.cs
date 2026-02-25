using Microsoft.AspNetCore.Mvc;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourScriptComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}