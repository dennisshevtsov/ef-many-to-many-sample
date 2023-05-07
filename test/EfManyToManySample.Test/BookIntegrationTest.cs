// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

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
  }
}
