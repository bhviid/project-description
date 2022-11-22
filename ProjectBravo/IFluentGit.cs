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
        Task<List<FrequencyDTO>> ThenReturnFrequencyDTOList();

        Task<float> ThenReturnAverageCommitsPerAuthorAsync();
    }
}