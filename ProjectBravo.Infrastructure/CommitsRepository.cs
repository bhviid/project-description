using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBravo.Infrastructure;

public class CommitsRepository : ICommitRepository
{
    public Task<CommitDTO> CreateAsync(CommitCreateDTO author)
    {
        throw new NotImplementedException();
    }

    public Task<CommitDTO?> FindAsync(int commitId)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<CommitDTO>> ReadAsync()
    {
        throw new NotImplementedException();
    }
}
