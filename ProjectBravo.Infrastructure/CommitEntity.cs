using System.Runtime.CompilerServices;

namespace ProjectBravo.Infrastructure;

public class CommitEntity{
    public int Id { get; set; }

    public int RepositoryId {get; set;}

    public virtual AuthorEntity Author {get; set;}

    public DateTime Date {get; set;}

    public string Message{get; set;}
    public string RepoName { get; set; }

    public string Sha { get; set; }

    //public CommitEntity(int RepositoryId, AuthorEntity Author, DateTime Date, string Message, string RepoName, string Sha)
    //{
    //    this.Sha = Sha;
    //    this.RepositoryId = RepositoryId;  
    //    this.Author = Author;
    //    this.RepoName = RepoName;
    //    this.Date = Date;
    //    this.Message = Message;

    //}


}
