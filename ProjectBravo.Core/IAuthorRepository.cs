namespace ProjectBravo.Core;

public interface IAuthorRepository
{
    Task<AuthorDetailsDTO> CreateAsync(AuthorCreateDTO author);
    Task<AuthorDTO?> FindAsync(int authorId);
    Task<IReadOnlyCollection<AuthorDTO>> ReadAsync();
    Task<AuthorDTO> UpdateAsync(AuthorUpdateDTO author);
    Task DeleteAsync(int authorId);
}