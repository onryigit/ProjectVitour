using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Driver;
using ProjectVitour.Dtos.ReviewDtos;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Review> _reviewCollection;

        public ReviewService(IMapper mapper,IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reviewCollection=database.GetCollection<Review>(databaseSettings.ReviewCollectionName);
            _mapper = mapper;
        }

        public async Task CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var values=_mapper.Map<Review>(createReviewDto);
            await _reviewCollection.InsertOneAsync(values);
        }

        public async Task DeleteReviewAsync(string id)
        {
            await _reviewCollection.DeleteOneAsync(x => x.ReviewId == id);
        }

        public async Task<List<ResultReviewDto>> GetAllReviewAsync()
        {
            var values = await _reviewCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultReviewDto>>(values);
        }

        public async Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string id)
        {
           var values=await _reviewCollection.Find(x=> x.TourId==id).ToListAsync();
            return _mapper.Map<List<ResultReviewByTourIdDto>>(values);
        }

        public async Task<GetReviewByIdDto> GetReviewByIdAsync(string id)
        {
            var values=await _reviewCollection.Find(x=>x.ReviewId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetReviewByIdDto>(values);
        }

        public async Task UpdateReviewAsync(UpdateReviewDto updateReviewDto)
        {
           var values = _mapper.Map<Review>(updateReviewDto);
            await _reviewCollection.FindOneAndReplaceAsync(x=>x.ReviewId==updateReviewDto.ReviewId,values);
        }
    }
}
