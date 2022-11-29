using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectBravo.Core;

public interface IGitRepoRepository
{
    public Task<Results<Created<GitRepository>, ValidationProblem>> CreateAsync(GitRepository gitRepository);

    public Task<Results<Ok<GitRepository>, NotFound<int>>> FindAsync(int repositoryId);

    public Task<IReadOnlyCollection<GitRepository>> ReadAsync();
    public Task<Results<NoContent, NotFound<int>>> DeleteAsync(int repositoryId);
    public Task<Results<NoContent, NotFound<int>>> UpdateAsync(int repoId, GitRepository gitRepository);


}