using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace ThestralService.Infrastructure.EntityFrameworkCore.DbContext.ModelBuilder;

using Domain.Transfer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class TransferModelBuilder
{
    public static void Configure(this EntityTypeBuilder<Transfer> builder)
    {
        builder.Property(p => p.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.UserId)
          .HasMaxLength(15);

        builder.Property(p => p.ExternalOperatorId)
          .HasMaxLength(200)
          .IsRequired();

        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.UserId).IsRequired();
        builder.HasIndex(p => p.Id).IsUnique();
        builder.HasIndex(p => p.UserId).IsUnique();
        builder.Property(p => p.Files)
            .HasColumnName("Files")
            .HasConversion(
                m => JsonSerializer.Serialize(m, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                m => JsonSerializer.Deserialize<Dictionary<string, string[]>>(m, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
            )
            .HasColumnType("jsonb");
    }
}
