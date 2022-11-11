using System.Globalization;
using System.Text;
using LibGit2Sharp;

namespace ProjectBravo;
public class GitInsights : IGitAnalyzer
{
    private static string dateFormat = "yyyy-MM-dd";

    public GitInsights()
    {
    }

    public List<IGrouping<DateTime, Commit>> GenerateCommitsByDate(string repository)
    {
        using var repo = new Repository(repository);
        return GenerateCommitsByDate(repo);
    }

    public List<IGrouping<DateTime, Commit>> GenerateCommitsByDate(Repository repository)
    {
        return repository.Commits.GroupBy(commit => commit.Author.When.Date).ToList();
    }

    public Dictionary<string, List<Commit>> GenerateCommitsByAuthor(string repoPath)
    {
        using var repo = new Repository(repoPath);
        return GenerateCommitsByAuthor(repo);
    }

    public Dictionary<string, List<Commit>> GenerateCommitsByAuthor(Repository repo)
    {
        Dictionary<string, List<Commit>> authorToCommits;

        var dateGroups = repo.Commits.GroupBy(
            commit => commit.Author.When.Date
        );
        authorToCommits = new Dictionary<string, List<Commit>>();

        var authors = repo.Commits.Select(commit => commit.Author.Name).Distinct();
        foreach (var author in authors)
        {
            authorToCommits.Add(author, new List<Commit>());
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
}