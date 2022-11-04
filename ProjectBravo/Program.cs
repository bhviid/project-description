using ProjectBravo.Infrastructure;
using CommandLine;

namespace ProjectBravo;

public class Program
{
    public class CommandLineParserOptions
    {
        [Value(0, Required = true, MetaName = "mode", HelpText = "Mode to run.")]
        public string Mode { get; set; }
        [Value(1, Required = true, MetaName = "repo", HelpText = "Path to git repository.")]
        public string Repository { get; set; }
    }

    public static void Main(string[] args)
    {

        var factory = new GitContextFactory();
        var context = factory.CreateDbContext(args);

        context.Repos.Add( new GitRepository{
            Name = "Etrepo",
            Authors = new HashSet<Author>{new Author{Name = "Frederik"}},
            Commits = new HashSet<Commit>{new Commit{Message = "HVASÅ", Date = DateTime.Now, Author = new Author{ Name = "asger"}}},
            LatestCommitDate = DateTime.Now,
        });
        context.SaveChanges();
        
        
        // var result = Parser.Default.ParseArguments<CommandLineParserOptions>(args)
        // .WithParsed(Run)
        // .WithNotParsed(HandleParseError);

    }

    private static void HandleParseError(IEnumerable<Error> errors)
    {
        if (errors.IsVersion())
        {
            Console.WriteLine("Version Request");
            return;
        }

        if (errors.IsHelp())
        {
            Console.WriteLine("Help Request");
            return;
        }
        Console.WriteLine("Parser Fail");
    }

    private static void Run(CommandLineParserOptions options)
    {
        if (options.Mode == "frequency")
        {
            GitInsights.PrintFrequencies(options.Repository);
        }
        if (options.Mode == "author")
        {
            GitInsights.PrintAuthors(options.Repository);
        }
    }

}