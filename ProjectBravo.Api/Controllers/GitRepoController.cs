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

    [HttpGet]
    public string Get() => "API is indeed alive";

    [HttpGet]
    [Route("frequency-test")]
    public IEnumerable<FrequencyDTO> GetFreqs()
    {
        return new[] {
        new FrequencyDTO(21, DateTime.Now.AddDays(-2)),
        new FrequencyDTO(5, DateTime.Now.AddDays(-5)),
        new FrequencyDTO(6, DateTime.Now.AddDays(-8)),
        new FrequencyDTO(2, DateTime.Now.AddDays(-12)),
        new FrequencyDTO(3, DateTime.Now)
        };
    }

    [HttpGet()]
    [Route("freqeuncy-dto/{github_user}/{repo_name}")]
    public async Task<List<FrequencyDTO>> GetFrequencyDTOs(string? github_user, string? repo_name, [FromServices] IGitHelper FluentBoi)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name!);
        List<FrequencyDTO> toReturn;

        if (foundInDb is null)
        {
            toReturn = await FluentBoi.CreateInstance(_dbrepo, _commitRepo)
                        .ThenCloneGitRepository(github_user!, repo_name!)
                        .ThenAddNewDbEntry()
                        .ThenReturnFrequencyDTOList();
        }
        else
        {
            var helper = FluentBoi.CreateInstance(_dbrepo, _commitRepo)
                                  .ThenCloneGitRepository(github_user!, repo_name!);

            if (helper.IsNewerThanInDb(foundInDb))
            {
                toReturn = await helper.ThenUpdateExistingDbEntry()
                                 .ThenReturnFrequencyDTOList();
            }
            else
            {
                toReturn = await helper.ThenGetCurrentFromDb()
                            .ThenReturnFrequencyDTOList();
            }
        }
        return toReturn;
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
}
