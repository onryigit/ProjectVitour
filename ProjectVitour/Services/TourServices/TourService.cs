using AutoMapper;
using MongoDB.Driver;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Dtos.TourImageDtos;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<TourPlan> _tourPlanCollection;
        private readonly IMongoCollection<TourImage> _tourImageCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _reviewCollection = database.GetCollection<Review>(_databaseSettings.ReviewCollectionName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _tourPlanCollection = database.GetCollection<TourPlan>(_databaseSettings.TourPlanCollectionName);
            _tourImageCollection = database.GetCollection<TourImage>(_databaseSettings.TourImageCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            var values = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(values);
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.TourID == id);
        }

        public async Task<List<ResultTourDto>> GetAllToursAsync()
        {
            var values = await _tourCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var tour = await _tourCollection.Find(x => x.TourID == id).FirstOrDefaultAsync();
            var tourDto = _mapper.Map<GetTourByIdDto>(tour);

            if (tourDto != null)
            {
                var plans = await _tourPlanCollection.Find(x => x.TourID == id)
                                                     .SortBy(x => x.DayNumber)
                                                     .ToListAsync();
                tourDto.TourPlans = _mapper.Map<List<TourPlanDto>>(plans);

                var reviews = await _reviewCollection.Find(x => x.TourId == id && x.Status == true).ToListAsync();
                tourDto.Reviews = _mapper.Map<List<ProjectVitour.Dtos.ReviewDtos.ResultReviewByTourIdDto>>(reviews);

                // --- CASE 3 MADDE 1.2: SHOT GALLERY ---
                var images = await _tourImageCollection.Find(x => x.TourID == id).ToListAsync();
                tourDto.TourImages = _mapper.Map<List<ResultTourImageDto>>(images);
                // --------------------------------------
            }

            return tourDto;
        }
        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x => x.TourID == updateTourDto.TourID, values);
        }

        public async Task<List<ResultTourDto>> GetToursWithPaginationAsync(int page, int pageSize)
        {
            var values = await _tourCollection.Find(x => true)
                                              .Skip((page - 1) * pageSize)
                                              .Limit(pageSize)
                                              .ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<long> GetTourCountAsync()
        {
            return await _tourCollection.CountDocumentsAsync(x => true);
        }

        public async Task<List<ResultTourDto>> GetFilteredToursAsync(string keyword, string categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize)
        {
            var builder = Builders<Tour>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter &= builder.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(keyword, "i")) | 
                          builder.Regex(x => x.Location, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                filter &= builder.Eq(x => x.CategoryID, categoryId);
            }
            if (minPrice.HasValue)
            {
                filter &= builder.Gte(x => x.Price, minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                filter &= builder.Lte(x => x.Price, maxPrice.Value);
            }

            var values = await _tourCollection.Find(filter)
                                              .Skip((page - 1) * pageSize)
                                              .Limit(pageSize)
                                              .ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<long> GetFilteredTourCountAsync(string keyword, string categoryId, decimal? minPrice, decimal? maxPrice)
        {
            var builder = Builders<Tour>.Filter;
            var filter = builder.Empty;

            if (!string.IsNullOrEmpty(keyword))
            {
                filter &= builder.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(keyword, "i")) | 
                          builder.Regex(x => x.Location, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                filter &= builder.Eq(x => x.CategoryID, categoryId);
            }
            if (minPrice.HasValue)
            {
                filter &= builder.Gte(x => x.Price, minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                filter &= builder.Lte(x => x.Price, maxPrice.Value);
            }

            return await _tourCollection.CountDocumentsAsync(filter);
        }
    }
}