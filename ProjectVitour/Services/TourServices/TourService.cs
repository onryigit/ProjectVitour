using AutoMapper;
using ProjectVitour.Dtos.TourDtos;

namespace ProjectVitour.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;

        public TourService(IMapper mapper)
        {
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

        public Task<List<ResultTourDto>> GetAllTourASync()
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
