using AutoMapper;
using MongoDB.Driver;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Entities;
using ProjectVitour.Settings;

namespace ProjectVitour.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _mapper = mapper;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
           var values = _mapper.Map<Tour>(createTourDto);
           await _tourCollection.InsertOneAsync(values); 
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x=>x.TourID==id);
        }

        public async Task<List<ResultTourDto>> GetAllToursAsync()
        {
            var values = await _tourCollection.Find(x=>true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
           var values= await _tourCollection.Find(x=>x.TourID == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(values);
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var values = _mapper.Map<Tour>(updateTourDto);
            await _tourCollection.FindOneAndReplaceAsync(x=>x.TourID==updateTourDto.TourID,values);
        }
    }
}
