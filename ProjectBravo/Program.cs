using ProjectBravo.Infrastructure;

namespace ProjectBravo;

public class Program
{
    public static void Main(string[] args)
    {
        //cmds til docker containeren er i GitContextFactory.cs, I kan springe migration over.
        // bare start docker container og ef database update.
        /*var factory = new GitContextFactory();
        using var context = factory.CreateDbContext(args);

        context.repos.Add(new GitRepository(){AuthorOutput = "Brr", FrequencyOutput = "brr", LatestCommitDate = DateTime.Now});
        context.SaveChanges();

        foreach (var item in context.repos)
        {
            Console.WriteLine($"ID: {item.Id} and msg: {item.AuthorOutput}");
        } */

        if (args[0] == "frequency")
        {
            GitInsights.PrintFrequencies(args[1]);
        }
        if (args[0] == "author")
        {
            GitInsights.PrintAuthors(args[1]);
        }
    }
}

