using AutoMapper;
using ProjectVitour.Dtos.CategoryDtos;
using ProjectVitour.Dtos.ReservationDtos;
using ProjectVitour.Dtos.ReviewDtos;
using ProjectVitour.Dtos.TourDtos;
using ProjectVitour.Dtos.TourImageDtos;
using ProjectVitour.Entities;

namespace ProjectVitour.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            CreateMap<Tour, CreateTourDto>().ReverseMap();
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<Tour, UpdateTourDto>().ReverseMap();
            CreateMap<Tour, GetTourByIdDto>().ReverseMap();

            CreateMap<Review, CreateReviewDto>().ReverseMap();
            CreateMap<Review, ResultReviewDto>().ReverseMap();
            CreateMap<Review, UpdateReviewDto>().ReverseMap();
            CreateMap<Review, GetReviewByIdDto>().ReverseMap();
            CreateMap<Review, ResultReviewByTourIdDto>().ReverseMap();

            CreateMap<TourPlan, TourPlanDto>().ReverseMap();

            CreateMap<Reservation, CreateReservationDto>().ReverseMap();
            CreateMap<Reservation, ResultReservationDto>().ReverseMap();
            CreateMap<Reservation, UpdateReservationDto>().ReverseMap();
            CreateMap<Reservation, GetReservationByIdDto>().ReverseMap();

            CreateMap<TourImage, CreateTourImageDto>().ReverseMap();
            CreateMap<TourImage, ResultTourImageDto>().ReverseMap();
            CreateMap<TourImage, UpdateTourImageDto>().ReverseMap();
            CreateMap<TourImage, GetTourImageByIdDto>().ReverseMap();
        }
    }
}
