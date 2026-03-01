using ProjectVitour.Models;

namespace ProjectVitour.Services.GeminiServices
{
    public interface IGeminiCostService
    {
        Task<GeminiCostResponse> CalculateHiddenCostAsync(string destination, int days, decimal tourPrice, string includedItems);
    }
}