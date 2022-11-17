using System.Security.Cryptography;

namespace ProjectBravo.Infrastructure;

public class AuthorsRepository : IAuthorRepository
{
    private readonly GitContext _context;
    public AuthorsRepository(GitContext context)
    {
        _context = context;
    }
    public async Task<AuthorDTO> CreateAsync(AuthorCreateDTO author)
    {
        var entity = new Author
        {
            Name = author.Name,
            Email = author.Email,
        };
        _context.Authors.Add(entity);
        await _context.SaveChangesAsync();

        return new AuthorDTO(entity.Id, entity.Name, entity.Email);

    }
    public async Task<AuthorDTO?> FindAsync(int authorId)
    {
        var entitiy = from ent in _context.Authors
                      where ent.Id == authorId
                      select new AuthorDTO(ent.Id, ent.Name);

        return await entitiy.FirstOrDefaultAsync();


    }
    public async Task<IReadOnlyCollection<AuthorDTO>> ReadAsync()
    {
        var entity = from ent in _context.Authors
                     orderby ent.Name
                     select new AuthorDTO(ent.Id, ent.Name);

        return await entity.ToListAsync();
    }
    public async Task<Status> UpdateAsync(AuthorDTO author)
    {
        var entity = await _context.Authors.FindAsync(author.Id);
        Status status;
        if (entity == null)
        {
            status = NotFound;

        }
        else if (await _context.Authors.FirstOrDefaultAsync(a => a.Id != author.Id && a.Name == author.Name) != null)
        {
            status = Conflict;
        }
        else
        {
            entity.Name = author.Name;
            await _context.SaveChangesAsync();
            status = Updated;

        }
        return status;

    }

    public async Task<Status> DeleteAsync(int authorId)
    {
        var entity = await _context.Authors.FindAsync(authorId);
        Status status;
        if (entity == null)
        {
            status = NotFound;
        }
        else
        {

            _context.Authors.Remove(entity);
            await _context.SaveChangesAsync();
            status = Deleted;
        }
        return status;
    }

}
