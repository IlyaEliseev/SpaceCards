using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceCards.DataAccess.Postgre.Entites;
using SpaceCards.Domain.Model;

namespace SpaceCards.DataAccess.Postgre.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Property(x => x.Email)
                .IsRequired(true);

            builder.Property(x => x.Nickname)
                .HasMaxLength(User.MaAX_NICKNAME_LENGTH)
                .IsRequired(true);

            builder.Property(x => x.PasswordHash)
                .IsRequired(true);

            builder.Property(x => x.RegistrationData)
                .IsRequired(true);
        }
    }
}
