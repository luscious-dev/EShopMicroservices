using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasConversion(productId => productId.Value, id => ProductId.Of(id));
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();

        }
    }
}
