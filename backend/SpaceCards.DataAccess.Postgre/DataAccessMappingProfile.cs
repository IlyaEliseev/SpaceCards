using AutoMapper;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre
{
    public class DataAccessMappingProfile : Profile
    {
        public DataAccessMappingProfile()
        {
            CreateMap<CardEntity, Card>().ReverseMap();
            CreateMap<GroupEntity, Group>().ReverseMap();
            CreateMap<CardGuessingStatisticsEntity, CardGuessingStatistics>().ReverseMap();
            CreateMap<UserEntity, User>().ReverseMap();
            CreateMap<SessionEntity, Session>().ReverseMap();

        }
    }
}
