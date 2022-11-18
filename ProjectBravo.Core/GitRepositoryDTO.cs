
namespace ProjectBravo.Core
{
    public record GitRepositoryDTO(int Id, string Name, DateTime LatestCommit, IList<AuthorDTO> Authors, IList<CommitCreateDTO> Commits);

    public record GitRepositryCreateDTO(
        string Name,
        IList<AuthorCreateDTO> Authors,
        IList<CommitCreateDTO> Commits
        );

    public record GitRepositryUpdateDTO(
        string Name,
        IList<AuthorCreateDTO> Authors,
        IList<CommitCreateDTO> NewCommits
        );
}
