using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PipeReports.API.Entities;

namespace PipeReports.API.Database.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Client)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property( x => x.Date)
               .IsRequired();

        builder.Property(x => x.Address)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.PostCode)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(100);
    }
}
