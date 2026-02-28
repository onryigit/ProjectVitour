using ProjectVitour.Entities;

namespace ProjectVitour.Services.GuideServices
{
    public interface IGuideService
    {
        Task<List<Guide>> GetAllGuidesAsync();
        Task CreateGuideAsync(Guide guide);
        Task UpdateGuideAsync(Guide guide);
        Task DeleteGuideAsync(string id);
        Task<Guide> GetGuideByIdAsync(string id);
    }
}