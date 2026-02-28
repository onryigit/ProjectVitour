using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Services.CategoryServices;

namespace ProjectVitour.ViewComponents.TourViewComponents
{
    public class _TourWidgetSelectComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _TourWidgetSelectComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            return View(categories);
        }
    }
}
