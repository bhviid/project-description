using LibGit2Sharp;
using System.Globalization;

public class Program
{
    public static void Main(string[] args)
    {
        if (args[1] == "frequency")
        {
            Frequency(args[0]);
        }
        if (args[1] == "author")
        {
            Author(args[0]);
        }
    }

    private static void Frequency(string repository)
    {
        using var repo = new Repository(repository);
            repo.Commits
                .GroupBy(commit => commit.Author.When.Date)
                .ToList()
                .ForEach(
                    x =>
                        Console.WriteLine(
                            $"{x.Count()} {x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"
                        )
                );
    }

    private static void Author(string repository)
    {
        using var repo = new Repository(repository);
                {
                    var dateFormat = "yyyy-MM-dd";
        
                    var dateGroups = repo.Commits.GroupBy(
                        commit => commit.Author.When.Date
                    );
                    Dictionary<string, List<Commit>> authorToCommits = new Dictionary<string, List<Commit>>();
        
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
        
                    foreach (var author in authorToCommits.Keys)
                    {
                        Console.WriteLine(author);
        
                        var authorDateCommits = authorToCommits[author].GroupBy(
                        commit => commit.Author.When.Date
                    );
                        foreach (var dateGroup in authorDateCommits)
                        {
                            Console.WriteLine($"\t{dateGroup.Count()} {dateGroup.Key.ToString(dateFormat, CultureInfo.InvariantCulture)}");
                        }
                        Console.WriteLine();
                    }
                }
    }
}
