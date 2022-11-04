namespace ProjectBravo.Infrastructure;

public class AuthorsRepository : IAuthorRepository
{
    public Task<AuthorDetailsDTO> CreateAsync(AuthorCreateDTO author)
    {
        throw new NotImplementedException();
    }
    public Task<AuthorDTO?> FindAsync(int authorId)
    {
        throw new NotImplementedException();
    }
    public Task<IReadOnlyCollection<AuthorDTO>> ReadAsync()
    {
        throw new NotImplementedException();
    }
    public Task<AuthorDTO> UpdateAsync(AuthorUpdateDTO author)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(int authorId)
    {
        throw new NotImplementedException();
    }

}
