

namespace ProjectBravo.Core;

public interface ICommitRepository
{
    Task<(Status, CommitDTO)> CreateAsync(CommitCreateDTO author);
    Task<(Status, CommitDTO?)> FindAsync(int commitId);
    Task<IReadOnlyCollection<CommitDTO>> ReadAsync();
}