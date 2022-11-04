namespace ProjectBravo.Core;

public interface IGitRepoRepository
{
    Task<GitRepositoryDTO> CreateAsync(GitRepositryCreateDTO gitRepo);

    Task<GitRepositoryDTO?> FindAsync(string gitRepoName);

    Task<IReadOnlyCollection<GitRepositoryDTO>> ReadAsync();
    Task<GitRepositoryDTO> UpdateAsync(GitRepositryUpdateDTO author);
    Task DeleteAsync(int authorId);
}