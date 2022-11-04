namespace ProjectBravo.Core
{
    public record AuthorDTO(
        int id,
        string name
    );

    public record AuthorDetailsDTO();

    public record AuthorCreateDTO();

    public record AuthorUpdateDTO();
}