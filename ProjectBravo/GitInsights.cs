using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using LibGit2Sharp;
using Newtonsoft.Json.Linq;
using ProjectBravo.Core;
using ProjectBravo.Infrastructure;

namespace ProjectBravo;
public class GitInsights : IGitAnalyzer
{
    private static string dateFormat = "yyyy-MM-dd";
    private static HttpClient Client;

    public GitInsights()
    {
        Client = new HttpClient();
        RunAsync().GetAwaiter().GetResult();
    }

    public List<IGrouping<DateTime, LibGit2Sharp.Commit>> GenerateCommitsByDate(string repository)
    {
        using var repo = new Repository(repository);
        return GenerateCommitsByDate(repo);
    }

    public List<IGrouping<DateTime, LibGit2Sharp.Commit>> GenerateCommitsByDate(Repository repository)
    {
        return repository.Commits.GroupBy(commit => commit.Author.When.Date).ToList();
    }

    public Dictionary<string, List<LibGit2Sharp.Commit>> GenerateCommitsByAuthor(string repoPath)
    {
        using var repo = new Repository(repoPath);
        return GenerateCommitsByAuthor(repo);
    }

    public Dictionary<string, List<LibGit2Sharp.Commit>> GenerateCommitsByAuthor(Repository repo)
    {
        Dictionary<string, List<LibGit2Sharp.Commit>> authorToCommits;

        var dateGroups = repo.Commits.GroupBy(
            commit => commit.Author.When.Date
        );
        authorToCommits = new Dictionary<string, List<LibGit2Sharp.Commit>>();

        var authors = repo.Commits.Select(commit => commit.Author.Name).Distinct();
        foreach (var author in authors)
        {
            authorToCommits.Add(author, new List<LibGit2Sharp.Commit>());
            foreach (var commit in repo.Commits)
            {
                if (commit.Author.Name == author)
                {
                    authorToCommits[author].Add(commit);
                }
            }
        }

        return authorToCommits;
    }

    public string GetFrequencyString(Repository repo)
    {
        var brr = GenerateCommitsByDate(repo);
        var sb = new StringBuilder();
        brr.ForEach(
            x =>
                sb.Append(
                    $"{x.Count()} {x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}\n"
                )
        );
        return sb.ToString();
    }

    public string GetFrequencyString(IEnumerable<CommitDTO> commits)
    {
        var sb = new StringBuilder();
        commits.GroupBy(c => c.Date).ToList()
        .ForEach(
            x =>
                sb.Append(
                    $"{x.Count()} {x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}\n"
                )
        );
        return sb.ToString();
    }

    public void PrintFrequencies(string repoPath)
    {
        var repository = new Repository(repoPath);
        Console.WriteLine(GetFrequencyString(repository));
    }

    public void PrintAuthors(string repository)
    {
        using var repo = new Repository(repository);
        Console.Write(GetAuthorString(repo));
    }

    public string GetAuthorString(Repository repo)
    {
        var sb = new StringBuilder();

        var authorCommits = GenerateCommitsByAuthor(repo);
        foreach (var author in authorCommits.Keys)
        {
            sb.Append(author + "\n");

            var authorDateCommits = authorCommits[author].GroupBy(
            commit => commit.Author.When.Date
        );
            foreach (var dateGroup in authorDateCommits)
            {
                sb.Append($"\t{dateGroup.Count()} {dateGroup.Key.ToString(dateFormat, CultureInfo.InvariantCulture)}\n");

                sb.Append("\n");
            }
        }
        return sb.ToString();
    }

    public Repository CloneGithubRepo(string githubuser, string repoName)
    {
        string path = Repository.Clone($"https://github.com/{githubuser}/{repoName}.git", "clonedRepo");
        return new Repository(path);
    }

    static async Task RunAsync()
    {
        Client.BaseAddress = new Uri("https://api.github.com");
        Client.DefaultRequestHeaders.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        //Client.DefaultRequestHeaders.Add("Authorization", "Bearer <TOKEN>");
    }

    public async Task<List<Fork>> GetRepoForks(string owner, string repo)
    {
        var forkList = new List<Fork>();
        var response = await Client.GetAsync($"/repos/{owner}/{repo}/forks");
        if (response.IsSuccessStatusCode)
        {
            var jForks = JArray.Parse(await response.Content.ReadAsStringAsync());
            foreach (JObject jFork in jForks)
            {
                var fork = new Fork();
                fork.Id = jFork.GetValue("id").Value<int>();
                fork.Name = jFork.GetValue("name").Value<string>();
                var forkOwner = new Author();
                forkOwner.Name = jFork.GetValue("owner.login").Value<string>();
                forkOwner.Id = jFork.GetValue("owner.id").Value<int>();
                fork.Owner = forkOwner;

                forkList.Add(fork);
            }
        }
        return forkList;
    }
}