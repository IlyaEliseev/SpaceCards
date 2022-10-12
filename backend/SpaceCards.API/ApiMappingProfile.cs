using AutoMapper;
using SpaceCards.Domain.Model;

namespace SpaceCards.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Card, Contracts.GetCardResponse>();
            CreateMap<Group, Contracts.GetGroupResponse>();
            CreateMap<CardGuessingStatistics, Contracts.GetCardGusessingStatisticsResponse>();
        }
    }
}
