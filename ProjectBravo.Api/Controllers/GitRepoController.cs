using Microsoft.AspNetCore.Mvc;

namespace ProjectBravo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GitRepoController : ControllerBase
{
    private readonly IGitRepoRepository _dbrepo;
    private readonly IGitAnalyzer _analyzer;
    public GitRepoController(IGitRepoRepository repo, IGitAnalyzer gitAnalyzer)
    {
        _dbrepo = repo;
        _analyzer = gitAnalyzer;
    }

    [HttpGet()]
    [Route("freqeuncy/{github_user}/{repo_name}")]
    public async Task<string> GetFrequency(string? github_user, string? repo_name)
    {
        var foundInDb = await _dbrepo.FindAsync(repo_name);
        string toReturn;

        if (foundInDb is null)
        {

            var cloned = _analyzer.CloneGithubRepo(github_user, repo_name);
            var authors = cloned.Commits.Select(c => c.Author.Name).Distinct().ToList();
            var commits = cloned.Commits.Select(c =>
                new CommitCreateDTO(c.Author.When.DateTime, c.Message, c.Author.Name, repo_name))
                .ToList();

            var s = await _dbrepo.CreateAsync(new GitRepositryCreateDTO(repo_name,
                authors, commits));

            toReturn = _analyzer.GetFrequencyString(cloned);
            
            //clone repo locally
            //add to database...
        }
        else
        {
            //check if the repo has been updated compared to our database, if yes update the database.
            //if(foundInDb.LatestCommit > repo_name)

            toReturn = "brr";
            //return new
            //{
            //    user,
            //    repo_name
            //};
        }

        return toReturn ;
    }
}
