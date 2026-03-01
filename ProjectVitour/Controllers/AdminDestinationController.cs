using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Entities;
using ProjectVitour.Services.DestinationServices;

namespace ProjectVitour.Controllers
{
    public class AdminDestinationController : Controller
    {
        private readonly IDestinationService _destinationService;

        public AdminDestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _destinationService.GetAllDestinationsAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateDestination()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDestination(Destination destination)
        {
            destination.Status = true;
            await _destinationService.CreateDestinationAsync(destination);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleStatus(string id)
        {
            var value = await _destinationService.GetDestinationByIdAsync(id);
            if (value != null)
            {
                value.Status = !value.Status;
                await _destinationService.UpdateDestinationAsync(value);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDestination(string id)
        {
            var value = await _destinationService.GetDestinationByIdAsync(id);
            if (value == null) return RedirectToAction("Index");
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDestination(Destination destination)
        {
            await _destinationService.UpdateDestinationAsync(destination);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteDestination(string id)
        {
            await _destinationService.DeleteDestinationAsync(id);
            return RedirectToAction("Index");
        }
    }
}