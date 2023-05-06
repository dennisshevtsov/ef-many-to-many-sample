// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore.ValueGeneration;

  /// <summary>Defines an entity type configuration for the <see cref="EfManyToManySample.AuthorEntity"/>.</summary>
  public sealed class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<AuthorEntity>
  {
    /// <summary>Configures the entity of type <see cref="EfManyToManySample.AuthorEntity"/>.</summary>
    /// <param name="builder">An object that provides a simple API for configuring an <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType" />.</param>
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
      builder.ToTable("author");
      builder.HasKey(entity => entity.AuthorId);

      builder.Property(entity => entity.AuthorId)
             .HasColumnName("id")
             .IsRequired()
             .ValueGeneratedOnAdd()
             .HasValueGenerator<GuidValueGenerator>();

      builder.Property(entity => entity.Name)
             .HasColumnName("name")
             .IsRequired()
             .HasMaxLength(256);

      builder.HasMany(entity => entity.Books)
             .WithMany(entity => entity.Authors)
             .UsingEntity<BookAuthorEntity>(
               "book_author",
               builder => builder.HasOne(entity => entity.Book)
                                 .WithMany()
                                 .HasForeignKey(entity => entity.BookId)
                                 .HasPrincipalKey(entity => entity.BookId),
               builder => builder.HasOne(entity => entity.Author)
                                 .WithMany()
                                 .HasForeignKey(entity => entity.AuthorId)
                                 .HasPrincipalKey(entity => entity.AuthorId)
             );
    }
  }
}
