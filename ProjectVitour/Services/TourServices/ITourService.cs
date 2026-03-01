using ProjectVitour.Dtos.TourDtos;

namespace ProjectVitour.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllToursAsync();
        Task CreateTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);
        Task<List<ResultTourDto>> GetToursWithPaginationAsync(int page, int pageSize);
        Task<long> GetTourCountAsync();
        Task<List<ResultTourDto>> GetFilteredToursAsync(string keyword, string categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize, string sort = null);
        Task<long> GetFilteredTourCountAsync(string keyword, string categoryId, decimal? minPrice, decimal? maxPrice);

    }
}
