// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace Microsoft.Extensions.DependencyInjection
{
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.Options;

  using EfManyToManySample;

  /// <summary>Extends an API of the <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.</summary>
  public static class ServicesExtensions
  {
    /// <summary>Sets up the databse.</summary>
    /// <param name="services">An object that specifies the contract for a collection of service descriptors.</param>
    /// <param name="configuration">An object that represents a set of key/value application configuration properties.</param>
    /// <returns>An object that specifies the contract for a collection of service descriptors.</returns>
    public static IServiceCollection SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<DatabaseOptions>(configuration);
      services.AddDbContext<DbContext, AppDbContext>((provider, builder) =>
      {
        var options = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        if (string.IsNullOrWhiteSpace(options.ConnectionString))
        {
          throw new ArgumentNullException(nameof(DatabaseOptions.ConnectionString));
        }

        builder.UseNpgsql(options.ConnectionString);
      });

      return services;
    }
  }
}
