using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class CardGuessingStatisticsConfiguration : IEntityTypeConfiguration<CardGuessingStatistics>
    {
        public void Configure(EntityTypeBuilder<CardGuessingStatistics> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CardId)
                .IsRequired(true);

            builder.Property(x => x.Success)
                .IsRequired(true);

            builder.Property(x => x.UserId)
                .IsRequired(true);
        }
    }
}
