using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.TourServices;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _AllTourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;

        public _AllTourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // URL'den "?page=2" gibi gelen değeri yakalıyoruz, gelmezse 1 kabul ediyoruz.
            int page = 1;
            if (int.TryParse(HttpContext.Request.Query["page"], out int p))
            {
                page = p;
            }

            int pageSize = 6; // Case 3: Her sayfada 6 tur listelenmelidir.

            // Sadece o sayfanın verilerini getir
            var values = await _tourService.GetToursWithPaginationAsync(page, pageSize);

            // Toplam sayfa sayısını hesapla
            long totalCount = await _tourService.GetTourCountAsync();
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // View tarafında kullanmak için ViewBag'e atıyoruz
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;

            return View(values);
        }
    }
}