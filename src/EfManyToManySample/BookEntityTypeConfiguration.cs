// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Metadata.Builders;
  using Microsoft.EntityFrameworkCore.ValueGeneration;

  /// <summary>Defines an entity type configuration for the <see cref="EfManyToManySample.BookEntity"/>.</summary>
  public sealed class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
  {
    /// <summary>Configures the entity of type <see cref="EfManyToManySample.BookEntity"/>.</summary>
    /// <param name="builder">An object that provides a simple API for configuring an <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType" />.</param>
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
      builder.ToTable("book");
      builder.HasKey(entity => entity.BookId);

      builder.Property(entity => entity.BookId)
             .HasColumnName("id")
             .IsRequired()
             .ValueGeneratedOnAdd()
             .HasValueGenerator<GuidValueGenerator>();

      builder.Property(entity => entity.Title)
             .HasColumnName("title")
             .IsRequired()
             .HasMaxLength(256);

      builder.HasMany(entity => entity.Authors)
             .WithMany(entity => entity.Books)
             .UsingEntity<BookAuthorEntity>(
               "book_author",
               builder => builder.HasOne(entity => entity.Author)
                                 .WithMany()
                                 .HasForeignKey(entity => entity.AuthorId)
                                 .HasPrincipalKey(entity => entity.AuthorId),
               builder => builder.HasOne(entity => entity.Book)
                                 .WithMany()
                                 .HasForeignKey(entity => entity.BookId)
                                 .HasPrincipalKey(entity => entity.BookId)
             );
    }
  }
}
