using ProjectBravo.Core;
using LibGit2Sharp;

namespace ProjectBravo
{
    public class GitHelper : IFreshGitHelper, IClonedGitHelper, IFinalGitHelper
    {
        private readonly IGitRepoRepository _dbRepoRepo;
        private readonly ICommitRepository _dbCommitRepo;

        private Repository? _libgitRepo;

        private GitRepositoryDTO _alreadyInDb;

        private string _githubUser;
        private string _gitRepoName;

        private ShouldDo _shouldDo;
        private IList<string> authorsToAdd;
        private IList<CommitCreateDTO> commitsToAdd;
        private IList<int> newCommitIds;

        internal GitHelper(IGitRepoRepository rRepo, ICommitRepository cRepo)
        {
            _dbRepoRepo = rRepo;
            _dbCommitRepo = cRepo;
        }

        public IFreshGitHelper CreateInstance(IGitRepoRepository gitRepoDbRepo, ICommitRepository gitCommitDbRepo)
        {
            return new GitHelper(gitRepoDbRepo, gitCommitDbRepo);
        }

        public IClonedGitHelper ThenCloneGitRepository(string user, string repoName)
        {
            _githubUser = user;
            _gitRepoName = repoName;
            _libgitRepo = CloneGithubRepo(user, repoName);
            return this;
        }

        private Repository CloneGithubRepo(string githubUser, string repoName)
        {
            string path = Repository.Clone($"https://github.com/{githubUser}/{repoName}.git", $"clonedRepos/{githubUser}/{repoName}");
            return new Repository(path);
        }

        public bool IsNewerThanInDb(GitRepositoryDTO that)
        {
            _alreadyInDb = that;
            return that.LatestCommit < _libgitRepo!.Commits.Max(c => c.Author.When.DateTime);
        }

        public IFinalGitHelper ThenAddNewDbEntry()
        {
            authorsToAdd = _libgitRepo!.Commits.Select(c => c.Author.Name).Distinct().ToList();
            commitsToAdd = _libgitRepo!.Commits.Select(c =>
                new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, _gitRepoName))
                .ToList();

            _shouldDo = ShouldDo.CreateNew;

            return this;
        }

        public IFinalGitHelper ThenUpdateExistingDbEntry()
        {
            _shouldDo = ShouldDo.UpdateExisting;

            authorsToAdd = _libgitRepo!.Commits.Select(c => c.Author.Name).Distinct().ToList();
            commitsToAdd = _libgitRepo!.Commits
                .Where(c => _alreadyInDb.LatestCommit <= c.Author.When.DateTime)
                .Select(c =>
                    new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, _gitRepoName))
                .ToList();

            return this;
        }
        public IFinalGitHelper ThenGetCurrentFromDb()
        {
            _shouldDo = ShouldDo.ReadExisting;

            return this;
        }

        public async Task<string> ThenReturnAuthorString()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CommitDTO>> ThenReturnCommits()
        {
            if (_shouldDo != ShouldDo.ReadExisting)
            {
                await PerformDbAction();
            }

            return await GetCommitsAsync();
        }

        public async Task<string> ThenReturnFrequencyString()
        {
            if (_shouldDo != ShouldDo.ReadExisting)
            {
                await PerformDbAction();
            }

            IGitAnalyzer analyzer = new GitInsights();
            return analyzer.GetFrequencyString(await GetCommitsAsync());
        }

        private async Task<IEnumerable<CommitDTO>> GetCommitsAsync()
        {
            var commitsInDb = await _dbCommitRepo.ReadAsync();
            return commitsInDb.Where(c => _alreadyInDb.CommitIds.Contains(c.Id)).Select(c => c);
        }

        public async Task<int> ThenReturnAverageCommitsPerAuthorAsync()
        {
            if (_shouldDo != ShouldDo.ReadExisting)
            {
                await PerformDbAction();
            }
            var commits = await GetCommitsAsync();

            var numAuthors = commits.Select(x => x.AuthorName).Distinct().Count();

            //can there be 0 authors?
            return commits.Count() / (numAuthors == 0 ? 1 : numAuthors);
        }

        private async Task PerformDbAction()
        {
            switch (_shouldDo)
            {
                case ShouldDo.CreateNew:
                    _alreadyInDb = await _dbRepoRepo.CreateAsync(new GitRepositryCreateDTO(_gitRepoName,
                        authorsToAdd, commitsToAdd));
                    break;
                case ShouldDo.UpdateExisting:
                    foreach (var commit in commitsToAdd)
                    {
                        var temp = commit;
                        var dto = await _dbCommitRepo.CreateAsync(temp);
                        newCommitIds.Add(dto.Id);
                    }
                    var updatedRepo = await _dbRepoRepo.UpdateAsync(new GitRepositryUpdateDTO(
                        _gitRepoName,
                        authorsToAdd,
                        newCommitIds
                    ));
                    _alreadyInDb = updatedRepo;
                    break;
            }
        }

        public async Task<List<FrequencyDTO>> ThenReturnFrequencyDTOList()
        {
            if (_shouldDo != ShouldDo.ReadExisting)
            {
                await PerformDbAction();
            }

            IGitAnalyzer analyzer = new GitInsights();
            return analyzer.GenerateFrequencyDTO(await GetCommitsAsync());
        }

        private enum ShouldDo
        {
            CreateNew, UpdateExisting, ReadExisting
        }

    }
    public class GitHelperInitializer : IGitHelper
    {
        public IFreshGitHelper CreateInstance(IGitRepoRepository gitRepo, ICommitRepository commitRepo)
        {
            return new GitHelper(gitRepo, commitRepo);
        }
    }

}