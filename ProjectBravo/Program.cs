using System.Globalization;
using LibGit2Sharp;
if (args.Length > 0)
{
    if (args.Length > 1 && args[1] == "frequency")
    {
        using (var repo = new Repository(args[0]))
        {
            var dateGroups = repo.Commits.GroupBy(
                commit => commit.Author.When.Date
            );
            var dateFormat = "yyyy-MM-dd";
            foreach (var dateGroup in dateGroups)
            {
                Console.WriteLine($"{dateGroup.Count()} {dateGroup.Key.ToString(dateFormat, CultureInfo.InvariantCulture)}");
            }
        }
    }
    //brr
    else if (args.Length > 1 && args[1] == "author")
    {
        using (var repo = new Repository(args[0]))
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
    else
    {
        Console.WriteLine(@"Please provide one of the following: 
            frequency
            author");
    }
}
else
{
    if (args.Length > 0)
    {
        Console.WriteLine($"No command found for {args[1]}.");
    }
    else
    {
        Console.WriteLine($"No command provided.");
    }

}
