using FormCollector.Domain;
using Microsoft.EntityFrameworkCore;

namespace FormCollector.Infrastructure.Data.Repositories;

internal sealed class SubmissionRepository(DbContext dbContext)
    : ISubmissionRepository
{
    private const int DefaultPageSize = 10;

    public async Task<Submission?> FindAsync(SubmissionId id, CancellationToken ct = default)
    {
        return await dbContext.Set<Submission>()
            .SingleOrDefaultAsync(s => s.Id.Key == id.Key, ct)
            .ConfigureAwait(false);
    }

    public async Task<(List<Submission> Items, int TotalCount)> FindPaginatedAsync(
        int page,
        int pageSize,
        List<string> terms,
        FormData? formData,
        CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = DefaultPageSize;

        var query = dbContext.Set<Submission>().AsQueryable();

        if (formData is not null)
        {
            query = query.Where(s =>
                s.FormData.Id.Value == formData.Id.Value &&
                s.FormData.Name == formData.Name);
        }

        foreach (var term in terms)
        {
            var pattern = $"%{term}%";
            query = query.Where(s =>
                EF.Functions.Like(s.Payload.Flattened, pattern)
            );
        }

        var totalCount = await query.CountAsync(ct).ConfigureAwait(false);

        var items = await query
            .OrderByDescending(s => s.CreatedOnUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);

        return (items, totalCount);
    }

    public async Task CreateAsync(Submission entity, CancellationToken ct = default)
    {
        await dbContext.Set<Submission>().AddAsync(entity, ct).ConfigureAwait(false);
    }

    public async Task StoreAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct).ConfigureAwait(false);   
    }
}
