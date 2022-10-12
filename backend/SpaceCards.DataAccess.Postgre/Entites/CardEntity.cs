namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class CardEntity
    {
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public string FrontSide { get; set; } = string.Empty;

        public string BackSide { get; set; } = string.Empty;

        public GroupEntity? Group { get; set; }

        public int? GroupId { get; set; }
    }
}
