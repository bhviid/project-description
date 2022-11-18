
namespace ProjectBravo.Core;
public record CommitDTO(
        int Id,
        DateTime Date,
        string Message,
        string AuthorName,
        int RepositoryId
    );

    public record CommitCreateDTO(
        //
        int RepositoryId,
        DateTime Date,
        string Message,
        string AuthorName,
        string Email,
        string RepoName
        
    );
