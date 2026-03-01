using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Models;
using ProjectVitour.Entities;
using ProjectVitour.Services.ContactMessageServices;
using System.Diagnostics;

namespace Project3ViTour.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IContactMessageService _contactMessageService;

    public HomeController(ILogger<HomeController> logger, IContactMessageService contactMessageService)
    {
        _logger = logger;
        _contactMessageService = contactMessageService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult About()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contact(ContactMessage contactModel)
    {
        await _contactMessageService.CreateMessageAsync(contactModel);
        TempData["SuccessMsg"] = "Mesajınız başarıyla gönderildi! En kısa sürede sizinle iletişime geçeceğiz.";
        return RedirectToAction("Contact");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
