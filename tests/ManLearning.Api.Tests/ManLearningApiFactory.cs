using ManLearning.Infrastructure.Persistence.EfCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ManLearning.Api.Tests;

/// <summary>
/// Boots the real <c>ManLearning.Api</c> host in-process (via <see cref="WebApplicationFactory{TEntryPoint}"/>)
/// so integration tests exercise the actual HTTP → Controller → Application → Infrastructure
/// pipeline, not just a directly-invoked controller method. The only thing replaced is the
/// database: production/dev use SQL Server or a SQLite file (see
/// <c>InfrastructureServiceCollectionExtensions.AddManLearningInfrastructure</c>), but tests swap
/// in an in-memory SQLite connection, mirroring the same approach
/// <c>ManLearning.Infrastructure.Tests</c> already uses for repository round-trip tests.
/// </summary>
public sealed class ManLearningApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public ManLearningApiFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                descriptor => descriptor.ServiceType == typeof(DbContextOptions<ManLearningDbContext>));
            if (dbContextOptionsDescriptor is not null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            services.AddDbContext<ManLearningDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
