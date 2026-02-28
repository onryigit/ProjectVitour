using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Entities;
using ProjectVitour.Services.GuideServices;

namespace ProjectVitour.Controllers
{
    public class AdminGuideController : Controller
    {
        private readonly IGuideService _guideService;

        public AdminGuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _guideService.GetAllGuidesAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateGuide()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGuide(Guide guide)
        {
            guide.Status = true;
            await _guideService.CreateGuideAsync(guide);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleStatus(string id)
        {
            var value = await _guideService.GetGuideByIdAsync(id);
            if (value != null)
            {
                value.Status = !value.Status;
                await _guideService.UpdateGuideAsync(value);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteGuide(string id)
        {
            await _guideService.DeleteGuideAsync(id);
            return RedirectToAction("Index");
        }
    }
}