namespace ProjectBravo.Core
{
    public record AuthorDTO(
        int Id,
        string Name,
        string Email
    );

    //public record AuthorDetailsDTO();

    public record AuthorCreateDTO(string Name, string Email);

    public record AuthorUpdateDTO(string newName, string Email);
}