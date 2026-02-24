using Project3ViTour.Dtos.ReviewDtos;

namespace Project3ViTour.Services.ReviewService
{
    public interface IReviewService
    {
        Task<List<ResultReviewDto>> GetAllReviewsAsync();
        Task CreateReviewAsync(CreateReviewDto createReviewDto);
        Task UpdateReviewAsync(UpdateReviewDto updateReviewDto);
        Task DeleteReviewAsync(string id);
        Task<GetReviewByIdDto> GetReviewByIdAsync(string id);
    }
}
