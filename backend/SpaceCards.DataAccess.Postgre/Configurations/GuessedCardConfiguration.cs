using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class GuessedCardConfiguration : IEntityTypeConfiguration<GuessedCard>
    {
        public void Configure(EntityTypeBuilder<GuessedCard> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CardId)
                .IsRequired(false);
        }
    }
}
