namespace ProjectBravo.Core
{
    public record CommitDTO(
        int Id,
        DateTime Date,
        string Message,
        string AuthorName,
        string RepoName
    );

    public record CommitCreateDTO(
        DateTime Date,
        string Message,
        string AuthorName,
        string RepoName
    );
}