using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.AccessToken)
                .HasMaxLength(Domain.Model.Session.MaxLengthToken)
                .IsRequired(true);

            builder.Property(x => x.RefreshToken)
                .HasMaxLength(Domain.Model.Session.MaxLengthToken)
                .IsRequired(true);
        }
    }
}
