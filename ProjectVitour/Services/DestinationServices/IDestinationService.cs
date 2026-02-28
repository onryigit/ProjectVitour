using ProjectVitour.Entities;

namespace ProjectVitour.Services.DestinationServices
{
    public interface IDestinationService
    {
        Task<List<Destination>> GetAllDestinationsAsync();
        Task CreateDestinationAsync(Destination destination);
        Task UpdateDestinationAsync(Destination destination);
        Task DeleteDestinationAsync(string id);
        Task<Destination> GetDestinationByIdAsync(string id);
    }
}