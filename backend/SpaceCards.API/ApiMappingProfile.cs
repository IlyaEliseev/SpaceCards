using AutoMapper;

namespace SpaceCards.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Domain.Card, Contracts.GetCardResponse>();
            CreateMap<Domain.Group, Contracts.GetGroupResponse>();
            CreateMap<Domain.CardGuessingStatistics, Contracts.GetCardGusessingStatisticsResponse>();
        }
    }
}
