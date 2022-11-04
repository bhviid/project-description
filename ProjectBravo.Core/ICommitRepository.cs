namespace ProjectBravo.Core;

public interface ICommitRepository
{
    Task<CommitDTO> CreateAsync(CommitCreateDTO author);
    Task<CommitDTO?> FindAsync(int commitId);
    Task<IReadOnlyCollection<CommitDTO>> ReadAsync();
}