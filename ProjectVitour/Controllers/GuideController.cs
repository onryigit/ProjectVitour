using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.GuideServices;

namespace ProjectVitour.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _guideService.GetAllGuidesAsync();
            var activeGuides = values.Where(x => x.Status).ToList();
            return View(activeGuides);
        }
    }
}