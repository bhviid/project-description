using Microsoft.AspNetCore.Mvc;

namespace ProjectBravo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GitRepoController : ControllerBase
{
    private readonly IGitRepoRepository _dbrepo;
    private readonly ICommitRepository _commitRepo;
    private readonly IGitAnalyzer _analyzer;
    public GitRepoController(IGitRepoRepository repo, ICommitRepository cRepo, IGitAnalyzer gitAnalyzer)
    {
        _dbrepo = repo;
        _commitRepo = cRepo;
        _analyzer = gitAnalyzer;
    }

    [HttpGet()]
    [Route("freqeuncy/{github_user}/{repo_name}")]
    public async Task<string> GetFrequency(string? github_user, string? repo_name)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        string toReturn;

        if (foundInDb is null)
        {
            //clone repo locally
            //add to database...

            var cloned = _analyzer.CloneGithubRepo(github_user!, repo_name!);


            var authors = cloned.Commits.Select(c => c.Author.Name).Distinct().ToList();
            var commits = cloned.Commits.Select(c =>
                new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, repo_name!))
                .ToList();

            var s = await _dbrepo.CreateAsync(new GitRepositryCreateDTO(repo_name,
                authors, commits));

            toReturn = _analyzer.GetFrequencyString(cloned);

        }
        else
        {
            //check if the repo has been updated compared to our database, if yes update the database.

            var cloned = _analyzer.CloneGithubRepo(github_user!, repo_name);
            var newestCommit = cloned.Commits.Max(x => x.Author.When.DateTime);

            if (foundInDb.LatestCommit < newestCommit)
            {
                var authors = cloned.Commits.Select(c => c.Author.Name).Distinct().ToList();
                var newCommits = cloned.Commits
                    .Where(c => foundInDb.LatestCommit <= c.Author.When.DateTime)
                    .Select(c =>
                        new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, repo_name))
                    .ToList();
                var commitIds = new List<int>();
                foreach (var commit in newCommits)
                {
                    var temp = commit;
                    var dto = await _commitRepo.CreateAsync(temp);
                    commitIds.Add(dto.Id);
                }

                var updatedRepo = await _dbrepo.UpdateAsync(new GitRepositryUpdateDTO(
                    repo_name,
                    authors,
                    commitIds
                ));

                foundInDb = updatedRepo;
            }
            // the up-to-date version is in our database.
            var commitsInDb = await _commitRepo.ReadAsync();
            var commitDTOs = commitsInDb.Where(c => foundInDb.CommitIds.Contains(c.Id)).Select(c => c);
            toReturn = _analyzer.GetFrequencyString(commitDTOs);
        }

        return toReturn;
    }

    [HttpGet()]
    [Route("freqeuncy-test/{github_user}/{repo_name}")]
    public async Task<string> GetFrequency2(string? github_user, string? repo_name)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        string toReturn;

        if (foundInDb is null)
        {
            toReturn = await GitHelper.CreateInstance(_dbrepo, _commitRepo)
                        .ThenCloneGitRepository(github_user!, repo_name!)
                        .ThenAddNewDbEntry()
                        .ThenReturnFrequencyString();
        }
        else
        {
            var helper = GitHelper.CreateInstance(_dbrepo, _commitRepo)
                                  .ThenCloneGitRepository(github_user!, repo_name!);
            
            if (helper.IsNewerThanInDb(foundInDb))
            {
                toReturn = await helper.ThenUpdateExistingDbEntry()
                                 .ThenReturnFrequencyString();
            }
            else 
            {
                toReturn = await helper.ThenGetCurrentFromDb()
                            .ThenReturnFrequencyString();
            }
        }
        return toReturn;
    }
}
