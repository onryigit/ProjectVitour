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
            // Paging
            int page = 1;
            if (int.TryParse(HttpContext.Request.Query["page"], out int p)) page = p;
            int pageSize = 6; 

            // Filters
            string search = HttpContext.Request.Query["search"];
            string categoryId = HttpContext.Request.Query["categoryId"];
            
            decimal? minPrice = null;
            if (decimal.TryParse(HttpContext.Request.Query["minPrice"], out decimal minP)) minPrice = minP;

            decimal? maxPrice = null;
            if (decimal.TryParse(HttpContext.Request.Query["maxPrice"], out decimal maxP)) maxPrice = maxP;

            var values = await _tourService.GetFilteredToursAsync(search, categoryId, minPrice, maxPrice, page, pageSize);
            long totalCount = await _tourService.GetFilteredTourCountAsync(search, categoryId, minPrice, maxPrice);
            
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalCount = totalCount;
            ViewBag.PageSize = pageSize;

            // Preserve query params for pagination links
            ViewBag.SearchQuery = search;
            ViewBag.CategoryIdQuery = categoryId;
            ViewBag.MinPriceQuery = minPrice;
            ViewBag.MaxPriceQuery = maxPrice;

            return View(values);
        }
    }
}