// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample.Test
{
  using Microsoft.EntityFrameworkCore;

  [TestClass]
  public sealed class AuthorIntegrationTest : IntegrationTestBase
  {
    [TestMethod]
    public void Add_NewAuthor_NewAuthorIdGenerated()
    {
      var controlAuthorEntity = new AuthorEntity
      {
        Name = Guid.NewGuid().ToString(),
      };

      DbContext.Add(controlAuthorEntity);

      Assert.AreNotEqual(default, controlAuthorEntity.AuthorId);
    }

    [TestMethod]
    public async Task SaveChangesAsync_NewAuthorAdded_NewAuthorSavedInDatabase()
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
                       .Where(entity => entity.AuthorId == controlAuthorEntity.AuthorId)
                       .SingleOrDefaultAsync();

      Assert.IsNotNull(actualAuthorEntity);
      Assert.AreEqual(controlAuthorEntity.Name, actualAuthorEntity.Name);
    }
  }
}
