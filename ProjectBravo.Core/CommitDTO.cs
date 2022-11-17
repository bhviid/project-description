
namespace ProjectBravo.Core;
public record CommitDTO(
        int Id,
        DateTime Date,
        string Message,
        string AuthorName,
        string RepoName
    );

    public record CommitCreateDTO(
        //
        string BelongsTo,
        DateTime Date,
        string Message,
        string AuthorName,
        string RepoName
        
    );
