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

        public TourService(IMapper mapper, DatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            _mapper = mapper;
        }

        public Task CreateTourAsync(CreateTourDto createTourDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTourAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ResultTourDto>> GetAllTourAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            throw new NotImplementedException();
        }
    }
}
