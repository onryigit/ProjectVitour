using ProjectVitour.Dtos.TourImageDtos;

namespace ProjectVitour.Services.TourImageServices
{
    public interface ITourImageService
    {
        Task<List<ResultTourImageDto>> GetAllTourImageAsync();
        Task CreateTourImageAsync(CreateTourImageDto createTourImageDto);
        Task UpdateTourImageAsync(UpdateTourImageDto updateTourImageDto);
        Task DeleteTourImageAsync(string id);
        Task<GetTourImageByIdDto> GetTourImageByIdAsync(string id);
        Task<List<ResultTourImageDto>> GetTourImagesByTourIdAsync(string id); // Madde 1.2 Shot Gallery için
    }
}