using Project3ViTour.Dtos.TourDtos;

namespace Project3ViTour.Services.TourService
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllToursAsync();
        Task CreateTourAsync(CreateTourDto createDtoTour);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);


    }
}
