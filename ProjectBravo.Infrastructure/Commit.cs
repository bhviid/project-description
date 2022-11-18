using System.Runtime.CompilerServices;

namespace ProjectBravo.Infrastructure;

public class Commit{
    public int Id { get; set; }

    public int RepositoryId {get; set;}

    public Author Author {get; set;}

    public DateTime Date {get; set;}

    public string Message{get; set;}
    public string RepoName { get; set; }

    public string Sha { get; set; }

    public Commit(int RepositoryId, Author Author, DateTime Date, string Message, string RepoName, string Sha)
    {
        this.Sha = Sha;
        this.RepositoryId = RepositoryId;  
        this.Author = Author;
        this.RepoName = RepoName;
        this.Date = Date;
        this.Message = Message;

    }


}
