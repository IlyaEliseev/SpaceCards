using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GroupId)
                .IsRequired(false);

            builder.Property(x => x.FrontSide)
                .HasMaxLength(Domain.Card.MAX_NAME_FRONTSIDE)
                .IsRequired(true);

            builder.Property(x => x.BackSide)
                .HasMaxLength(Domain.Card.MAX_NAME_BACKSIDE)
                .IsRequired(true);
        }
    }
}
