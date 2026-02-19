using Microsoft.AspNetCore.Mvc;
using ProjectVitour.Dtos.CategoryDtos;
using ProjectVitour.Services.CategoryServices;

namespace ProjectVitour.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>CreateCategory(CreateCategoryDto createCategoryDto)
        {
            createCategoryDto.CategoryStatus = true;
            await _categoryService.CreateCategoryAsync(createCategoryDto);
            return RedirectToAction("CategoryList");
        }
    }
}
