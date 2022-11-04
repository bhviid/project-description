namespace ProjectBravo.Infrastructure;

public class GitRepoRepository : IGitRepoRepository
{
    private readonly GitContext _context;

    public GitRepoRepository(GitContext context) => _context = context;
    
    public Task<GitRepositoryDTO> CreateAsync(GitRepositryCreateDTO gitRepo)
    {
        throw new NotImplementedException();
    }

    public Task<GitRepositoryDTO?> FindAsync(string gitRepoName)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<GitRepositoryDTO>> ReadAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GitRepositoryDTO> UpdateAsync(GitRepositryUpdateDTO author)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int authorId)
    {
        throw new NotImplementedException();
    }
}

