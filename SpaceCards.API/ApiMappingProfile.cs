using AutoMapper;

namespace SpaceCards.API
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
               CreateMap<Domain.Card, Contracts.GetUsersResponse>();
               CreateMap<Domain.Group, Contracts.GetGroupsResponse>();
        }
    }
}
