using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace uMini.Infrastructure.EntityConfigurations;

internal class ShortUrlEntityConfiguration : IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("ShortUrls", "shorturl");

        builder.HasKey(su => su.Id);

        builder.HasIndex(su => su.Key)
            .IsUnique(true);

        builder.HasIndex(su => su.CreatorId);

        builder.Property(x => x.Id)
            .UseHiLo("shorturlseq");

        builder.Property(su => su.Key)
            .HasMaxLength(20)
            .IsRequired(true);

        builder.Property(su => su.Url)
            .HasMaxLength(1000)
            .IsRequired(true);

        builder.Property(su => su.CreatorId)
            .IsRequired(false);

        builder.Property(su => su.CreatedDate)
            .IsRequired(true);
    }
}
