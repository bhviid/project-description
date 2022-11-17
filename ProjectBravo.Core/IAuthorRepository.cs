namespace ProjectBravo.Core;

public interface IAuthorRepository
{
    Task<AuthorDTO> CreateAsync(AuthorCreateDTO author);
    Task<AuthorDTO?> FindAsync(int authorId);
    Task<IReadOnlyCollection<AuthorDTO>> ReadAsync();
    Task<Status> DeleteAsync(int authorId);
    Task<Status> UpdateAsync(AuthorDTO author);
}