// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  /// <summary>Represents a book.</summary>
  public sealed class BookEntity
  {
    /// <summary>Initializes a new instance of the <see cref="EfManyToManySample.BookEntity"/> class.</summary>
    public BookEntity()
    {
      Title = string.Empty;
    }

    /// <summary>Gets/sets an object that represents an ID of a book.</summary>
    public Guid BookId { get; set; }

    /// <summary>Gets/sets an object that represents a title of a book.</summary>
    public string Title { get; set; }
  }
}
