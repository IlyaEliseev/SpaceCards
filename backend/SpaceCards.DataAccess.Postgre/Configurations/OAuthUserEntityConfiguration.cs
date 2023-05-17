using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class OAuthUserEntityConfiguration : IEntityTypeConfiguration<OAuthUserEntity>
    {
        public void Configure(EntityTypeBuilder<OAuthUserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Email)
                .HasMaxLength(OAuthUser.MAX_EMAIL_LENGTH)
                .IsRequired(true);

            builder.Property(x => x.Nickname)
                .HasMaxLength(OAuthUser.MAX_NICKNAME_LENGTH)
                .IsRequired(true);

            builder.Property(x => x.RegistrationData)
                .IsRequired(true);

            builder.Property(x => x.DeleteDate)
                .IsRequired(false);

            builder.HasQueryFilter(x => x.DeleteDate == null);

            builder.HasOne(x => x.Token)
                .WithOne(x => x.User)
                .HasForeignKey<OAuthUserTokenEntity>(x => x.OAuthUserId);
        }
    }
}
