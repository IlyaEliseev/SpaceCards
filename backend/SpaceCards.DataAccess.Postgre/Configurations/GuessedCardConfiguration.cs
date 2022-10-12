using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class GuessedCardConfiguration : IEntityTypeConfiguration<GuessedCardEntity>
    {
        public void Configure(EntityTypeBuilder<GuessedCardEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CardId)
                .IsRequired(false);
        }
    }
}
