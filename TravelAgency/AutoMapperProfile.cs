using AutoMapper;
using AutoMapper.EquivalencyExpression;
using TravelAgency.Dto;
using TravelAgency.Models;

namespace TravelAgency
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Place, PlaceDto>();
            CreateMap<PlaceDto, Place>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<TripDto, Trip>()
                .ForMember(opts => opts.ArrivalPlaceId,
                    opts => opts.MapFrom((src, dest, srcMember) =>
                        dest.ArrivalPlaceId = src.ArrivalPlaceId != 0 ? src.ArrivalPlaceId : dest.ArrivalPlaceId ))  
                .ForMember(opts => opts.DeparturePlaceId,
                    opts => opts.MapFrom((src, dest, srcMember) =>
                        dest.DeparturePlaceId = src.DeparturePlaceId != 0 ? src.DeparturePlaceId : dest.DeparturePlaceId ))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}