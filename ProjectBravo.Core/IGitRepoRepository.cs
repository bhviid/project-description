namespace ProjectBravo.Core;

public interface IGitRepoRepository
{
    Task<GitRepositoryDTO> CreateAsync(GitRepositryCreateDTO gitRepo);

    Task<GitRepositoryDTO?> FindAsync(string gitRepoName);

    // and so on?
}