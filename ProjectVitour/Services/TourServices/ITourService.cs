using ProjectVitour.Dtos.TourDtos;

namespace ProjectVitour.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllTourASync();
        Task CreateTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);

    }
}
