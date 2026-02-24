using Project3ViTour.Dtos.CategoryDtos;

namespace Project3ViTour.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(string id);
        Task<GetCatgoryByIdDto> GetCategoryByIdAsync(string id);
    }
}
