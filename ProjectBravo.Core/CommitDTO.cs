namespace ProjectBravo.Core
{
    public record CommitDTO(
        int id,
        DateTime date,
        string message,
        string authorName,
        string repoName
    );

    public record CommitCreateDTO(
        DateTime date,
        string message,
        string authorName,
        string repoName
    );
}