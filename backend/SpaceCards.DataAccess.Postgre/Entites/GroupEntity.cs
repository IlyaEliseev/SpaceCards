namespace SpaceCards.DataAccess.Postgre.Entites
{
    public class GroupEntity
    {
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<CardEntity> Cards { get; set; } = new List<CardEntity>();
    }
}
