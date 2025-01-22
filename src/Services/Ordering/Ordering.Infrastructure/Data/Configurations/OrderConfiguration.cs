using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasConversion(orderId => orderId.Value, id => OrderId.Of(id));
            builder.Property(x => x.TotalPrice).HasPrecision(18, 2);

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();

            builder.HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey(x => x.OrderId);

            builder.ComplexProperty(o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(x => x.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.ComplexProperty(o => o.ShippingAddress, shippingAddressBuilder =>
            {
                shippingAddressBuilder.Property(x => x.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                shippingAddressBuilder.Property(x => x.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                shippingAddressBuilder.Property(x => x.EmailAddress)
                    .HasMaxLength(50);

                shippingAddressBuilder.Property(x => x.AddressLine)
                    .HasMaxLength(100)
                    .IsRequired();

                shippingAddressBuilder.Property(x => x.Country)
                    .HasMaxLength(50);

                shippingAddressBuilder.Property(x => x.State)
                    .HasMaxLength(50);

                shippingAddressBuilder.Property(x => x.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

            builder.ComplexProperty(o => o.BillingAddress, billingAddressBuilder =>
            {
                billingAddressBuilder.Property(x => x.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                billingAddressBuilder.Property(x => x.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                billingAddressBuilder.Property(x => x.EmailAddress)
                    .HasMaxLength(50);

                billingAddressBuilder.Property(x => x.AddressLine)
                    .HasMaxLength(100)
                    .IsRequired();

                billingAddressBuilder.Property(x => x.Country)
                    .HasMaxLength(50);

                billingAddressBuilder.Property(x => x.State)
                    .HasMaxLength(50);

                billingAddressBuilder.Property(x => x.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

            builder.ComplexProperty(o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(x => x.CardNumber)
                    .IsRequired();

                paymentBuilder.Property(x => x.CVV)
                    .HasMaxLength(3)
                    .IsRequired();

                paymentBuilder.Property(x => x.Expiration)
                    .HasMaxLength(10);

                paymentBuilder.Property(x => x.PaymentMethod);

                paymentBuilder.Property(x => x.CardName)
                    .HasMaxLength(100);
            });
        }
    }
}
