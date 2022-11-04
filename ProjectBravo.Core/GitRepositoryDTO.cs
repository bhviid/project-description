namespace ProjectBravo.Core
{
    public record GitRepositoryDTO(int Id, string Name, DateTime LatestCommit, IList<string> Authors, IList<int> CommitIds);

    public record GitRepositryCreateDTO(
        string Name,
        IList<string> Authors,
        IList<CommitCreateDTO> Commits
        );

    public record GitRepositryUpdateDTO(
        string Name,
        IList<string> Authors,
        IList<int> NewCommitIds
        );
}
