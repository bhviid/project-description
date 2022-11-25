namespace ProjectBravo.Core
{
    public record ForkDTO(
        int Id,
        string Name,
        int AuthorId,
        string AuthorName
    );
}
