using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Profiles;

public class RestaurantProfile : Profile
{
    public RestaurantProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
            .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
            .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(r => r.Address, c => c.MapFrom(dto => new Address()
            { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));
        
        CreateMap<Dish, DishDto>();
        CreateMap<CreateDishDto, Dish>();
    }
}