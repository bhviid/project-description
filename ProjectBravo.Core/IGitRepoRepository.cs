namespace ProjectBravo.Core;

public interface IGitRepoRepository
{
    Task<GitRepositoryDetailsDTO> CreateAsync(GitRepositryCreateDTO gitRepo);

    Task<GitRepositoryDetailsDTO?> FindAsync(string gitRepoName);

    // and so on?
}