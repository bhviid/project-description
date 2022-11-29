using ProjectBravo.Core;
using LibGit2Sharp;
using ProjectBravo.Infrastructure;
using Commit = ProjectBravo.Core.Commit;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections;

namespace ProjectBravo
{
    public class GitHelper : IFreshGitHelper, IClonedGitHelper, IFinalGitHelper
    {
        private readonly IGitRepoRepository _dbRepoRepo;
        private readonly ICommitRepository _dbCommitRepo;

        private Repository? _libgitRepo;

        private GitRepository _alreadyInDb;

        private string _githubUser;
        private string _gitRepoName;
        private int _repoInDBId;

        private ShouldDo _shouldDo;
        private IList<Author> authorsToAdd;
        private IList<Commit> commitsToAdd;
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

        public bool IsNewerThanInDb(GitRepository that)
        {
            _alreadyInDb = that;
            return that.LatestCommitDate < _libgitRepo!.Commits.Max(c => c.Author.When.DateTime);
        }

        public IFinalGitHelper ThenAddNewDbEntry()
        {
            
            authorsToAdd = (IList<Author>)_libgitRepo!.Commits.Select(c =>
            new Author { Name = c.Author.Name, Email = c.Author.Email }
            );
            commitsToAdd = (IList<Core.Commit>)_libgitRepo!.Commits.Select(c =>
                new Commit
                {
                    
                    Author = new Author { Name = c.Author.Name, Email = c.Author.Email },
                    Message = c.Message,
                    Date = c.Author.When.DateTime,
                    RepoName = _gitRepoName,
                    RepositoryId = _repoInDBId,
                    Sha = c.Sha,

                });
                //( _repoInDBId,
                //c.Author.When.DateTime,
                //c.Message, c.Author.Name,
                //c.Author.Email,
                //_gitRepoName))
                

            _shouldDo = ShouldDo.CreateNew;

            return this;
        }

        public async Task<IFinalGitHelper> ThenUpdateExistingDbEntry()
        {
            _shouldDo = ShouldDo.UpdateExisting;
            var results = await _dbRepoRepo.FindAsync(_repoInDBId);
            Ok<GitRepository> found = results.Result as Ok<GitRepository>;
            var RepoInDb = found!.Value;

            authorsToAdd = (IList<Author>)_libgitRepo!.Commits.Select(c => new Author { 
                Name = c.Author.Name,
                Email = c.Author.Email,
            });

            commitsToAdd = (IList<Commit>)_libgitRepo!.Commits
                .Where(c => _alreadyInDb.LatestCommitDate <= c.Author.When.DateTime)
                .Select(c => new Commit
                {
                    RepositoryId = RepoInDb!.Id,
                    Date = c.Author.When.DateTime,
                    Author = new Author { Name = c.Author.Name, Email = c.Author.Email},
                    Message = c.Message,
                    Sha = c.Sha,
                    RepoName = _gitRepoName,
                });
                 

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

        public async Task<IEnumerable<Core.Commit>> ThenReturnCommits()
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

        private async Task<IEnumerable<Core.Commit>> GetCommitsAsync()
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
                    var repo = await _dbRepoRepo.CreateAsync(new Core.GitRepository
                    {
                        Name = _gitRepoName,
                        Authors = (HashSet<Author>)authorsToAdd,
                        Commits = (HashSet<Commit>)commitsToAdd,
                        LatestCommitDate = getLatestCommit(commitsToAdd),
                    });
                    var resultAsRepo = repo.Result as Created<GitRepository>;
                    _alreadyInDb = resultAsRepo!.Value!;
                    break;
                case ShouldDo.UpdateExisting:
                    foreach (var commit in commitsToAdd)
                    {
                        var temp = commit;
                        var results = await _dbCommitRepo.CreateAsync(temp);
                        var com = results.Result as Created<Commit>;
                        newCommitIds.Add(com!.Value!.Id);
                    }
                    var result = await _dbRepoRepo.UpdateAsync(_repoInDBId, new GitRepository {
                        Name = _gitRepoName,
                        Authors = (HashSet<Author>)authorsToAdd,
                        Commits = (HashSet<Commit>)commitsToAdd,
                        LatestCommitDate = getLatestCommit(commitsToAdd),
                    });
                    var found = await _dbRepoRepo.FindAsync(_repoInDBId);
                    var tempo = found.Result as Ok<GitRepository>;

                    _alreadyInDb = tempo!.Value;
                    break;
            }
        }

        private enum ShouldDo
        {
            CreateNew, UpdateExisting, ReadExisting
        }

        private DateTime getLatestCommit(IEnumerable<Commit> commits)
        {
            var latestDateTime = DateTime.MinValue;
            foreach (var commit in commits)
            {
                if (commit.Date > latestDateTime) { latestDateTime = commit.Date; }
            }
            return latestDateTime;
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