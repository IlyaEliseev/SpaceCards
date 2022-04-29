using AutoMapper;

namespace SpaceCards.DataAccess.Postgre
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<Entites.Card, Domain.Card>().ReverseMap();
            CreateMap<Entites.Group, Domain.Group>().ReverseMap();
        }
    }
}
