using System.Globalization;
using System.Text;
using LibGit2Sharp;

namespace ProjectBravo;
public static class GitInsights
{
    private static string dateFormat = "yyyy-MM-dd";
    public static List<IGrouping<DateTime, Commit>> GenerateCommitsByDate(string repository)
    {
        using var repo = new Repository(repository);
        return repo.Commits.GroupBy(commit => commit.Author.When.Date).ToList();
    }

    public static List<IGrouping<DateTime, Commit>> GenerateCommitsByDate(Repository repository)
    {
        return repository.Commits.GroupBy(commit => commit.Author.When.Date).ToList();
    }

    public static Dictionary<string, List<Commit>> GenerateCommitsByAuthor(string repository)
    {
        Dictionary<string, List<Commit>> authorToCommits;
        using var repo = new Repository(repository);
        {
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
        }
    return authorToCommits;
    }

    public static string GetFrequencyString(List<IGrouping<DateTime, Commit>> brr)
    {
        var sb = new StringBuilder();
        brr.ForEach(
            x =>
                sb.Append(
                    $"{x.Count()} {x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}\n"
                )
        );
        return sb.ToString();
    }

    public static void PrintFrequencies(string repository)
    {
        Console.WriteLine(GetFrequencyString(GenerateCommitsByDate(repository)));
    }

    public static void PrintAuthors(string repository)
    {
        var authorCommits = GenerateCommitsByAuthor(repository);
        foreach (var author in authorCommits.Keys)
        {
            Console.WriteLine(author);

            var authorDateCommits = authorCommits[author].GroupBy(
            commit => commit.Author.When.Date
        );
            foreach (var dateGroup in authorDateCommits)
            {
                Console.WriteLine($"\t{dateGroup.Count()} {dateGroup.Key.ToString(dateFormat, CultureInfo.InvariantCulture)}");
            }
            Console.WriteLine();
        }
    }
    public static Repository CloneGithubRepo(string githubuser, string repoName)
    {
        string path = Repository.Clone($"https://github.com/{githubuser}/{repoName}.git", "clonedRepo");
        return new Repository(path);
    }
}