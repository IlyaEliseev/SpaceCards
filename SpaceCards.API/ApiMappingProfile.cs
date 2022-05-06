using AutoMapper;

namespace SpaceCards.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
               CreateMap<Domain.Card, Contracts.GetCardsResponse>();
               CreateMap<Domain.Group, Contracts.GetGroupsResponse>();
        }
    }
}
