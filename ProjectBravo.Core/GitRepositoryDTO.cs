namespace ProjectBravo.Core
{
    public record GitRepositoryDTO(int id, string name, DateTime latestCommit);

    public record GitRepositoryDetailsDTO();
    public record GitRepositryCreateDTO();

    public record GitRepositryUpdateDTO();
}
