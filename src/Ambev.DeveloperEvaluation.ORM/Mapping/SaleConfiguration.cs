using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder.Property(s => s.Quantity)
                .IsRequired();

            builder.Property(s => s.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.CustomerId)
                .IsRequired();

            builder.Property(s => s.SellerId)
                .IsRequired();

            builder.Property(s => s.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Customer)
                .WithMany()
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Seller)
                .WithMany()
                .HasForeignKey(s => s.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}