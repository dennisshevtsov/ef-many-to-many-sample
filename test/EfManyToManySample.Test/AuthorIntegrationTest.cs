﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample.Test
{
  using Microsoft.EntityFrameworkCore;

  [TestClass]
  public sealed class AuthorIntegrationTest : IntegrationTestBase
  {
    [TestMethod]
    public void Add_NewAuthor_AuthorIdGenerated()
    {
      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      DbContext.Add(controlAuthorEntity);

      Assert.AreNotEqual(default, controlAuthorEntity.AuthorId);
    }

    [TestMethod]
    public async Task SaveChangesAsync_NewAuthorAdded_NewAuthorSaved()
    {
      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      var controlAuthorEntityEntry = DbContext.Add(controlAuthorEntity);
      await DbContext.SaveChangesAsync();
      controlAuthorEntityEntry.State = EntityState.Detached;

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Where(entity => entity.AuthorId == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    }

    [TestMethod]
    public async Task SaveChangesAsync_AttachedBooksAddedToAuthor_BookAuthorRelationsSaved()
    {
      var controlBookEntityCollection = new List<BookEntity>
      {
        new BookEntity { Title = Guid.NewGuid().ToString() },
        new BookEntity { Title = Guid.NewGuid().ToString() },
        new BookEntity { Title = Guid.NewGuid().ToString() },
      };

      DbContext.AddRange(controlBookEntityCollection);
      await DbContext.SaveChangesAsync();

      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      foreach (var bookEntity in controlBookEntityCollection)
      {
        controlAuthorEntity.Books.Add(bookEntity);
      }

      var controlAuthorEntityEntry = DbContext.Entry(controlAuthorEntity);
      controlAuthorEntityEntry.State = EntityState.Added;
      await DbContext.SaveChangesAsync();
      controlAuthorEntityEntry.State = EntityState.Detached;

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Include(entity => entity.Books)
                       .Where(entity => entity.AuthorId == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlBookEntityCollection.Count, actualAuthorEntity.Books.Count);

      foreach (var bookEntity in controlBookEntityCollection)
      {
        Assert.IsTrue(actualAuthorEntity.Books.Any(entity => entity.BookId == bookEntity.BookId));
      }
    }

    [TestMethod]
    public async Task SaveChangesAsync_DetachedBooksAddedToAuthor_BookAuthorRelationsNotSaved()
    {
      var controlBookEntityCollection = new List<BookEntity>
      {
        new BookEntity { Title = Guid.NewGuid().ToString() },
        new BookEntity { Title = Guid.NewGuid().ToString() },
        new BookEntity { Title = Guid.NewGuid().ToString() },
      };

      DbContext.AddRange(controlBookEntityCollection);
      await DbContext.SaveChangesAsync();

      foreach (var bookEntity in controlBookEntityCollection)
      {
        DbContext.Entry(bookEntity).State = EntityState.Detached;
      }

      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      foreach (var bookEntity in controlBookEntityCollection)
      {
        controlAuthorEntity.Books.Add(bookEntity);
      }

      var controlAuthorEntityEntry = DbContext.Entry(controlAuthorEntity);
      controlAuthorEntityEntry.State = EntityState.Added;
      await DbContext.SaveChangesAsync();
      controlAuthorEntityEntry.State = EntityState.Detached;

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Include(entity => entity.Books)
                       .Where(entity => entity.AuthorId == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(0, actualAuthorEntity.Books.Count);
    }

    [TestMethod]
    public async Task SaveChangesAsync_BookAuthorsAddedToAuthor_BookAuthorRelationsSaved()
    {
      var controlBookEntityCollection = await AddDetachedBooksAsyns(3);
      var controlAuthorEntity = await AddDetachedAuthorAsync();

      var controlBookAuthorEntityCollection =
        controlBookEntityCollection.Select(entity => new BookAuthorEntity(entity.BookId, controlAuthorEntity.AuthorId))
                                   .ToList();

      var controlAuthorEntityEntry = DbContext.Entry((object)controlAuthorEntity);
      controlAuthorEntityEntry.State = EntityState.Unchanged;

      var bookAuthorEntityCollectionNavigationEntry =
        controlAuthorEntityEntry.Navigation(nameof(AuthorEntity.BookAuthors));

      bookAuthorEntityCollectionNavigationEntry.CurrentValue = controlBookAuthorEntityCollection;
      bookAuthorEntityCollectionNavigationEntry.IsModified = true;

      await DbContext.SaveChangesAsync();

      var actualAuthorEntity =
        await DbContext.Set<AuthorEntity>()
                       .AsNoTracking()
                       .Include(entity => entity.Books)
                       .Where(entity => entity.AuthorId == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlBookEntityCollection.Count, actualAuthorEntity.Books.Count);
    }

    private async Task<List<BookEntity>> AddDetachedBooksAsyns(int books)
    {
      var controlBookEntityCollection = new List<BookEntity>();

      for (int i = 0; i < books; i++)
      {
        controlBookEntityCollection.Add(new BookEntity
        {
          Title = Guid.NewGuid().ToString(),
        });
      }

      DbContext.AddRange(controlBookEntityCollection);
      await DbContext.SaveChangesAsync();

      foreach (var bookEntity in controlBookEntityCollection)
      {
        DbContext.Entry(bookEntity).State = EntityState.Detached;
      }

      return controlBookEntityCollection;
    }

    private async Task<AuthorEntity> AddDetachedAuthorAsync()
    {
      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      var controlAuthorEntityEntry = DbContext.Entry(controlAuthorEntity);
      controlAuthorEntityEntry.State = EntityState.Added;
      await DbContext.SaveChangesAsync();
      controlAuthorEntityEntry.State = EntityState.Detached;

      return controlAuthorEntity;
    }
  }
}
