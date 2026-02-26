using ProjectVitour.Dtos.ReviewDtos;

namespace ProjectVitour.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<List<ResultReviewDto>> GetAllReviewAsync();
        Task CreateReviewAsync(CreateReviewDto createReviewDto);
        Task UpdateReviewAsync(UpdateReviewDto updateReviewDto);
        Task DeleteReviewAsync(string id);
        Task <GetReviewByIdDto> GetReviewByIdAsync(string id);
        Task<List<ResultReviewByTourIdDto>>GetAllReviewsByTourIdAsync(string id);
    }
}
