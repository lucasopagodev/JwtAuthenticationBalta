using JwtStore.Core.Contexts.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtStore.Infra.Contexts.AccountContext.Mappings;
public class RoleMap : IEntityTypeConfiguration<Role>
{
  public void Configure(EntityTypeBuilder<Role> builder)
  {
    builder.ToTable("Role");
    builder.HasKey(x => x.Id);
    builder.Property(x => x.Name)
      .HasColumnName("Name")
      .HasColumnName("NVARCHAR")
      .HasMaxLength(120)
      .IsRequired(true);
  }
}
