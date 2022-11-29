using ProjectBravo.Core;

namespace ProjectBravo
{
    public interface IGitHelper
    {
        IFreshGitHelper CreateInstance(IGitRepoRepository gitRepo, ICommitRepository commitRepo);
    }

    public interface IFreshGitHelper
    {
        IClonedGitHelper ThenCloneGitRepository(string user, string repo_name);
    }

    public interface IClonedGitHelper
    {
        bool IsNewerThanInDb(GitRepository gitRepo);
        IFinalGitHelper ThenAddNewDbEntry();

        Task<IFinalGitHelper>ThenUpdateExistingDbEntry();
        IFinalGitHelper ThenGetCurrentFromDb();
    }

    public interface IFinalGitHelper
    {
        Task<IEnumerable<Commit>> ThenReturnCommits();
        Task<string> ThenReturnFrequencyString();

        Task<string> ThenReturnAuthorString();
    }
}