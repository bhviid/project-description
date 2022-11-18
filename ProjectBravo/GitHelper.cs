using ProjectBravo.Core;
using LibGit2Sharp;
using ProjectBravo.Infrastructure;

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
        private int _repoInDBId;

        private ShouldDo _shouldDo;
        private IList<AuthorCreateDTO> authorsToAdd;
        private IList<CommitCreateDTO> commitsToAdd;
        private IList<int> newCommitIds;

        internal GitHelper(IGitRepoRepository rRepo, ICommitRepository cRepo)
        {
            _dbRepoRepo = rRepo;
            _dbCommitRepo = cRepo;
            var (status, RepoInDB) = _dbRepoRepo.FindAsync(_gitRepoName).Result;
            _repoInDBId = RepoInDB.Id;
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
            
            authorsToAdd = _libgitRepo!.Commits.Select(c =>
            new AuthorCreateDTO(c.Author.Name, c.Author.Email)).ToList();
            commitsToAdd = _libgitRepo!.Commits.Select(c =>
                new CommitCreateDTO( _repoInDBId, c.Author.When.DateTime, c.Message, c.Author.Name, c.Author.Email, _gitRepoName))
                .ToList();

            _shouldDo = ShouldDo.CreateNew;

            return this;
        }

        public IFinalGitHelper ThenUpdateExistingDbEntry()
        {
            _shouldDo = ShouldDo.UpdateExisting;
            var (status, RepoInDB) = _dbRepoRepo.FindAsync(_gitRepoName).Result;

            authorsToAdd = _libgitRepo!.Commits.Select(c => new AuthorCreateDTO(
                c.Author.Name,
                c.Author.Email)).Distinct().ToList();   
            commitsToAdd = _libgitRepo!.Commits
                .Where(c => _alreadyInDb.LatestCommit <= c.Author.When.DateTime)
                .Select(c =>
                    new CommitCreateDTO(RepoInDB.Id, c.Author.When.DateTime, c.Message, c.Author.Name, c.Author.Email, _gitRepoName))
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
            return commitsInDb.Where(c => c.RepositoryId == _repoInDBId).ToList();
            // Frederiks version:
            //return commitsInDb.Where(c => _alreadyInDb.Commits.Contains(c.Id)).Select(c => c);
        }

        private async Task PerformDbAction()
        {
            switch (_shouldDo)
            {
                case ShouldDo.CreateNew:
                    var (status, dto) = await _dbRepoRepo.CreateAsync(new GitRepositryCreateDTO(_gitRepoName,
                        authorsToAdd, commitsToAdd));
                    _alreadyInDb = dto;
                    break;
                case ShouldDo.UpdateExisting:
                    foreach (var commit in commitsToAdd)
                    {
                        var temp = commit;
                        var (HttpStatus, dto) = await _dbCommitRepo.CreateAsync(temp);
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