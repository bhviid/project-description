namespace ProjectBravo.Core;

public interface IGitRepoRepository
{
    Task<(Status, GitRepositoryDTO)> CreateAsync(GitRepositryCreateDTO gitRepo);

    Task<(Status, GitRepositoryDTO)> FindAsync(string gitRepoName);

    Task<IReadOnlyCollection<GitRepositoryDTO>> ReadAsync();
    Task<Status> UpdateAsync(GitRepositryUpdateDTO author);
    Task<Status> DeleteAsync(int authorId);
}