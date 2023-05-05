// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  /// <summary>Represents a relation between books and authors.</summary>
  public sealed class BookAuthorEntity
  {
    /// <summary>Gets/sets an object that represents an ID of a book.</summary>
    public Guid BookId { get; set; }

    /// <summary>Gets/sets an object that represents a book.</summary>
    public BookEntity? Book { get; set; }

    /// <summary>Gets/sets an object that represents an ID of an author.</summary>
    public Guid AuthorId { get; set; }

    /// <summary>Gets/sets an object that represents an author.</summary>
    public AuthorEntity? Author { get; set; }

  }
}
