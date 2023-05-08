// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample
{
  /// <summary>Represents an author.</summary>
  public sealed class AuthorEntity
  {
    /// <summary>Initializes a new instance of the <see cref="EfManyToManySample.AuthorEntity"/> class.</summary>
    public AuthorEntity()
    {
      Name  = string.Empty;
      Books = new List<BookEntity>();
    }

    /// <summary>Gets/sets an object that represents an ID of an author.</summary>
    public Guid AuthorId { get; set; }

    /// <summary>Gets/sets an object that represents a name of an author.</summary>
    public string Name { get; set; }

    /// <summary>Gets/sets an object that represents a collection of this author's books.</summary>
    public ICollection<BookEntity> Books { get; set; }

    /// <summary>Gets/sets an object that represents a collection of relations between books and this author.</summary>
    public ICollection<BookAuthorEntity> BookAuthors { get; set; }
  }
}
