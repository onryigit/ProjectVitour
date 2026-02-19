using AutoMapper;
using ProjectVitour.Dtos.CategoryDtos;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Entities;

namespace ProjectVitour.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping() 
        {
            CreateMap<Category,CreateCategoryDto>().ReverseMap();
            CreateMap<Category,ResultCategoryDto>().ReverseMap();
            CreateMap<Category,UpdateCategoryDto>().ReverseMap();
            CreateMap<Category,GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();

        }
    }
}
