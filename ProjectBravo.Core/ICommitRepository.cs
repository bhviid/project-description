

using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectBravo.Core;

public interface ICommitRepository
{
    Task<Results<Created<Commit>, ValidationProblem>> CreateAsync(Commit commit);
    Task<Results<Ok<Commit>, NotFound<int>>> FindAsync(int commitId);
    Task<IReadOnlyCollection<Commit>> ReadAsync();
}