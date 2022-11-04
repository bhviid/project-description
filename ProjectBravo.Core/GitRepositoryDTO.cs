namespace ProjectBravo.Core
{
    public record GitRepositoryDTO(int id, string name, DateTime latestCommit, IList<string> authors, IList<int> commitIds);

    public record GitRepositryCreateDTO(
        string name,
        IList<string> authors,
        IList<CommitCreateDTO> commits
        );

    public record GitRepositryUpdateDTO();
}
