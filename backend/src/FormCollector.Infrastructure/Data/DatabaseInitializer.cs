using AutoFixture;
using FormCollector.Application.Abstract;
using FormCollector.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FormCollector.Infrastructure.Data;

internal sealed class DatabaseInitializer
    : IDatabaseInitializer
{
    private readonly DbContext _dbContext;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        DbContext dbContext, 
        ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task InitializeAsync(bool recreateDatabase = false, CancellationToken ct = default)
    {
        _logger.LogInformation("Applying migrations for {DbContext}...", typeof(DbContext).Name);

        try
        {
            if (recreateDatabase)
            {
                await _dbContext.Database.EnsureDeletedAsync(ct).ConfigureAwait(false);
            }
            await _dbContext.Database.MigrateAsync(ct).ConfigureAwait(false);

            _logger.LogInformation("✅ Migrations applied successfully for {DbContext}.", typeof(DbContext).Name);
        
            await FillWithTestDataAsync(ct).ConfigureAwait(false);

            _logger.LogInformation("✅ Test data successfully created for {DbContext}.", typeof(DbContext).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Migration failed for {DbContext}.", typeof(DbContext).Name);
            throw;
        }
    }

    private async Task FillWithTestDataAsync(CancellationToken ct = default)
    {
        var fixture = new Fixture();

        for (int i = 0; i < 30; i++)
        {
            var client = fixture.Create<ClientModel>();

            var json = JsonSerializer.Serialize(client);

            var submission = new Submission(
                new FormData(new FormId("client-form"), "Create New Client"),
                Payload.FromJson(json));

            await _dbContext.Set<Submission>().AddAsync(submission, ct).ConfigureAwait(false);
        }
        
        await _dbContext.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    class ClientModel
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
    }
}

