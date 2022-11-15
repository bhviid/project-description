using ProjectBravo.Core;

namespace ProjectBravo
{

    public interface IFreshGitHelper
    {
        IClonedGitHelper ThenCloneGitRepository(string user, string repo_name);
    }

    public interface IClonedGitHelper
    {
        bool IsNewerThanInDb(GitRepositoryDTO gitRepo);
        IFinalGitHelper ThenAddNewDbEntry();

        IFinalGitHelper ThenUpdateExistingDbEntry();
        IFinalGitHelper ThenGetCurrentFromDb();
    }

    public interface IFinalGitHelper
    {
        Task<IEnumerable<CommitDTO>> ThenReturnCommits();
        Task<string> ThenReturnFrequencyString();

        Task<string> ThenReturnAuthorString();
    }
}