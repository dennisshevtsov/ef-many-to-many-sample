// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  using Microsoft.EntityFrameworkCore;

  /// <summary>Represents a session with the database and can be used to query and save instances of your entities.</summary>
  public sealed class AppDbContext : DbContext
  {
    /// <summary>Initializes a new instance of the <see cref="EfManyToManySample.AppDbContext"/> class.</summary>
    /// <param name="options">An object that represents options of a database context.</param>
    public AppDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
    }
  }
}
