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

        // 1. Yeni TourPlan koleksiyonumuzu tanımlıyoruz
        private readonly IMongoCollection<TourPlan> _tourPlanCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);

            // 2. TourPlan koleksiyonunu veritabanından alıyoruz
            _tourPlanCollection = database.GetCollection<TourPlan>(_databaseSettings.TourPlanCollectionName);

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

        // 3. GetTourByIdAsync metodunu güncelliyoruz
        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            // Önce ana tur bilgisini çekiyoruz
            var tour = await _tourCollection.Find(x => x.TourID == id).FirstOrDefaultAsync();
            var tourDto = _mapper.Map<GetTourByIdDto>(tour);

            if (tourDto != null)
            {
                // Tura ait olan gün gün planları (TourID ile ilişkili olarak) buluyoruz ve DayNumber'a göre sıralıyoruz
                var plans = await _tourPlanCollection.Find(x => x.TourID == id)
                                                     .SortBy(x => x.DayNumber)
                                                     .ToListAsync();

                // Bulunan planları DTO'daki listemize mapleyerek atıyoruz
                tourDto.TourPlans = _mapper.Map<List<TourPlanDto>>(plans);
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
    }
}