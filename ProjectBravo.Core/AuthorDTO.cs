namespace ProjectBravo.Core
{
    public record AuthorDTO(
        int Id,
        string Name
    );

    //public record AuthorDetailsDTO();

    public record AuthorCreateDTO(string Name);

    public record AuthorUpdateDTO(string newName);
}