using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

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
    public async Task<string> GetFrequency(string? github_user, int repoId, string? repo_name, [FromServices] IGitHelper FluentBoi)
    {
        var found = await _dbrepo.FindAsync(repoId);
        var result = found.Result as Ok<GitRepository>;
        string toReturn;

        if (result!.Value is null)
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
            
            if (helper.IsNewerThanInDb(result.Value))
            {
                //toReturn = await helper.ThenUpdateExistingDbEntry()
                await helper.ThenUpdateExistingDbEntry();
                return "Succes";



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
    public async Task<string> GetAuthor(string github_user, int repo_id, string repo_name, [FromServices] IGitHelper fluentBoi)
    {
        var result = await _dbrepo.FindAsync(repo_id);
        var found = result.Result as Ok<GitRepository>;
        string toReturn;

        if(found!.Value is null)
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

            if(cloned.IsNewerThanInDb(found.Value))
            {
                //toReturn = await cloned.ThenUpdateExistingDbEntry()
                //            .ThenReturnAuthorString();
                await cloned.ThenUpdateExistingDbEntry();
                return "Succes on updating";
            }
            else 
            {
                toReturn = await cloned.ThenGetCurrentFromDb()
                            .ThenReturnAuthorString();
            }
        }
        return toReturn;
    }
}
