// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

using Microsoft.EntityFrameworkCore;

namespace EfManyToManySample.Test
{
  [TestClass]
  public sealed class BookIntegrationTest : IntegrationTestBase
  {
    [TestMethod]
    public void Add_NewBook_BookIdGenerated()
    {
      var controlBookEntity = new BookEntity
      {
        Title = Guid.NewGuid().ToString(),
      };

      DbContext.Add(controlBookEntity);

      Assert.AreNotEqual(default, controlBookEntity.BookId);
    }

    [TestMethod]
    public async Task SaveChangesAsync_NewBookAdded_NewBookSaved()
    {
      var controlBookEntity = new BookEntity
      {
        Title = Guid.NewGuid().ToString(),
      };

      var controlBookEntityEntry = DbContext.Add(controlBookEntity);
      await DbContext.SaveChangesAsync();
      controlBookEntityEntry.State = EntityState.Detached;

      var actualBookEntity =
        await DbContext.Set<BookEntity>()
                       .Where(entity => entity.BookId == controlBookEntity.BookId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualBookEntity);
      Assert.AreEqual(controlBookEntity.Title, actualBookEntity.Title);
    }
  }
}
