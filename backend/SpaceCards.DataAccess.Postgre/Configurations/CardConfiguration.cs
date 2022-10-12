using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class CardConfiguration : IEntityTypeConfiguration<CardEntity>
    {
        public void Configure(EntityTypeBuilder<CardEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.GroupId)
                .IsRequired(false);

            builder.Property(x => x.FrontSide)
                .HasMaxLength(Domain.Model.Card.MAX_NAME_FRONTSIDE)
                .IsRequired(true);

            builder.Property(x => x.BackSide)
                .HasMaxLength(Domain.Model.Card.MAX_NAME_BACKSIDE)
                .IsRequired(true);

            builder.Property(x => x.UserId)
                .IsRequired(true);
        }
    }
}
