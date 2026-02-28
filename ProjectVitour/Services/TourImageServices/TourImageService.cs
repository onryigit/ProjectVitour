using AutoMapper;
using MongoDB.Driver;
using ProjectVitour.Dtos.TourImageDtos;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.TourImageServices
{
    public class TourImageService : ITourImageService
    {
        private readonly IMongoCollection<TourImage> _tourImageCollection;
        private readonly IMapper _mapper;

        public TourImageService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourImageCollection = database.GetCollection<TourImage>(_databaseSettings.TourImageCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourImageAsync(CreateTourImageDto createTourImageDto)
        {
            var value = _mapper.Map<TourImage>(createTourImageDto);
            await _tourImageCollection.InsertOneAsync(value);
        }

        public async Task DeleteTourImageAsync(string id)
        {
            await _tourImageCollection.DeleteOneAsync(x => x.TourImageID == id);
        }

        public async Task<List<ResultTourImageDto>> GetAllTourImageAsync()
        {
            var values = await _tourImageCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourImageDto>>(values);
        }

        public async Task<GetTourImageByIdDto> GetTourImageByIdAsync(string id)
        {
            var value = await _tourImageCollection.Find(x => x.TourImageID == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourImageByIdDto>(value);
        }

        public async Task<List<ResultTourImageDto>> GetTourImagesByTourIdAsync(string id)
        {
            var values = await _tourImageCollection.Find(x => x.TourID == id).ToListAsync();
            return _mapper.Map<List<ResultTourImageDto>>(values);
        }

        public async Task UpdateTourImageAsync(UpdateTourImageDto updateTourImageDto)
        {
            var values = _mapper.Map<TourImage>(updateTourImageDto);
            await _tourImageCollection.FindOneAndReplaceAsync(x => x.TourImageID == updateTourImageDto.TourImageID, values);
        }
    }
}