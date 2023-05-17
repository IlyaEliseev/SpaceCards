using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class OAuthUserTokenEntityConfiguration : IEntityTypeConfiguration<OAuthUserTokenEntity>
    {
        public void Configure(EntityTypeBuilder<OAuthUserTokenEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AccessToken)
               .HasMaxLength(OAuthUserToken.MAX_ACCESS_TOKEN_LENGTH)
               .IsRequired(true);

            builder.Property(x => x.RefreshToken)
               .HasMaxLength(OAuthUserToken.MAX_REFRESH_TOKEN_LENGTH)
               .IsRequired(true);

            builder.Property(x => x.ExpireTime)
                .IsRequired(true);
        }
    }
}
