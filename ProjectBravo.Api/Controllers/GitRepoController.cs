using Microsoft.AspNetCore.Mvc;

namespace ProjectBravo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GitRepoController : ControllerBase
{
    private readonly IGitRepoRepository _dbrepo;
    private readonly ICommitRepository _commitRepo;
    
    public GitRepoController(IGitRepoRepository repo, ICommitRepository cRepo)
    {
        _dbrepo = repo;
        _commitRepo = cRepo;
    }

    [HttpGet()]
    [Route("freqeuncy/{github_user}/{repo_name}")]
    public async Task<string> GetFrequency(string? github_user, string? repo_name, [FromServices] IGitHelper FluentBoi)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        string toReturn;

        if (foundInDb is null)
        {
            toReturn = await FluentBoi.CreateInstance(_dbrepo, _commitRepo)
                        .ThenCloneGitRepository(github_user!, repo_name!)
                        .ThenAddNewDbEntry()
                        .ThenReturnFrequencyString();
        }
        else
        {
            var helper = FluentBoi.CreateInstance(_dbrepo, _commitRepo)
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

    [HttpGet()]
    [Route("author/{github_user}/{repo_name}")]
    public async Task<string> GetAuthor(string github_user, string repo_name, [FromServices] IGitHelper fluentBoi)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        string toReturn;

        if(foundInDb is null)
        {
            toReturn = await fluentBoi.CreateInstance(_dbrepo, _commitRepo)
                        .ThenCloneGitRepository(github_user, repo_name)
                        .ThenAddNewDbEntry()
                        .ThenReturnAuthorString();
        }
        else 
        {
            var cloned = fluentBoi.CreateInstance(_dbrepo, _commitRepo)
                            .ThenCloneGitRepository(github_user, repo_name);

            if(cloned.IsNewerThanInDb(foundInDb))
            {
                toReturn = await cloned.ThenUpdateExistingDbEntry()
                            .ThenReturnAuthorString();
            }
            else 
            {
                toReturn = await cloned.ThenGetCurrentFromDb()
                            .ThenReturnAuthorString();
            }
        }
        return toReturn;
    }

    [HttpGet]
    [Route("forks/{github_user}/{repo_name}")]
    public async Task<List<ForkDTO>> GetForks(string github_user, string repo_name, [FromServices] IGitAnalyzer analyzer)
    {
        return await analyzer.GetRepoForks(github_user, repo_name);
    }

    [HttpGet()]
    [Route("average-commits/{github_user}/{repo_name}")]
    public async Task<int> GetAverageCommits(string github_user, string repo_name, [FromServices] IGitHelper fluentBoi)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        string toReturn;

        if(foundInDb is null)
        {
            return await fluentBoi.CreateInstance(_dbrepo, _commitRepo)
                        .ThenCloneGitRepository(github_user, repo_name)
                        .ThenAddNewDbEntry()
                        .ThenReturnAverageCommitsPerAuthorAsync();
        }
        else 
        {
            var cloned = fluentBoi.CreateInstance(_dbrepo, _commitRepo)
                            .ThenCloneGitRepository(github_user, repo_name);

            if(cloned.IsNewerThanInDb(foundInDb))
            {
                return await cloned.ThenUpdateExistingDbEntry()
                            .ThenReturnAverageCommitsPerAuthorAsync();
            }
            else 
            {
                return await cloned.ThenGetCurrentFromDb()
                            .ThenReturnAverageCommitsPerAuthorAsync();
            }
        }
    }
}
