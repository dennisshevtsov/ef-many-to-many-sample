// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace EfManyToManySample.Test
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  [TestClass]
  public abstract class IntegrationTestBase
  {
#pragma warning disable CS8618
    private IServiceScope _scope;

    protected DbContext DbContext { get; private set; }
#pragma warning restore CS8618

    [TestInitialize]
    public void Initialize()
    {
      var scope = new ServiceCollection().SetUpDatabase()
                                         .BuildServiceProvider()
                                         .CreateScope();

      DbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
      _scope = scope;
    }

    [TestCleanup]
    public void Cleanup()
    {
      DbContext?.Database.EnsureDeleted();
      _scope?.Dispose();
    }
  }
}
