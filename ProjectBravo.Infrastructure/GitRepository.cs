namespace ProjectBravo.Infrastructure;

public class GitRepository
{
    public int Id { get; set; }

    public string Name {get; set; }

    public HashSet<Author> Authors {get; set; }

    public HashSet<Commit> Commits {get; set;}

    public DateTime LatestCommitDate { get; set; }

   



    //public string FrequencyOutput { get; set; }

    //public string AuthorOutput { get; set; }

    
    /*
    public IList<Author> Collaborators { get; set; }
    public IList<Commit> Commits { get; set; }

    public IDictionary<Date,int> DateToAmountOfCommits { get; set; }
    public string Name { get; set; }
    public int MostRecentCommit { get; set; }
    */
    
}