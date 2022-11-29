using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectBravo.Infrastructure;

public class AuthorsRepository : IAuthorRepository
{
    private readonly GitContext _context;
    private readonly AuthorValidator _validator;
    public AuthorsRepository(GitContext context, AuthorValidator validator)
    {
        _context = context;
        _validator = validator;

    }
    public async Task<Results<Created<Author>, ValidationProblem>> CreateAsync(Author author)
    {
        var validation = _validator.Validate(author);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = new AuthorEntity
        {
            Name = author.Name,
            Email = author.Email,
        };
        _context.Authors.Add(entity);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"{entity.Id}", author with { Id = entity.Id });

    }
    public async Task<Results<Ok<Author>, NotFound<int>>> FindAsync(int authorId)
    {
        var entitiy = from ent in _context.Authors
                      where ent.Id == authorId
                      select new Author
                      {
                          Id= ent.Id,
                          Email = ent.Email,
                          Name= ent.Name
                      };

        var author = await entitiy.FirstOrDefaultAsync();
        return author is null ? TypedResults.NotFound(authorId) : TypedResults.Ok(author);


    }
    public async Task<IReadOnlyCollection<Author>> ReadAsync()
    {
        var entity = from ent in _context.Authors
                     orderby ent.Name
                     select new Author { Name = ent.Name, Email = ent.Email, Id = ent.Id};

        return await entity.ToListAsync();
    }
    public async Task<Results<NoContent, NotFound<int>>> UpdateAsync(int id, Author author)
    {
        var entity = await _context.Authors.FindAsync(id);
        
        if (entity == null)
        {
            return  TypedResults.NotFound(id);
        }
        entity.Name = author.Name;
        entity.Email = author.Email;

        await _context.SaveChangesAsync();
        return TypedResults.NoContent();
        

    }

    public async Task<Results<NoContent, NotFound<int>>> DeleteAsync(int authorId)
    {
        var entity = await _context.Authors.FindAsync(authorId);
        
        if (entity == null)
        {
            return TypedResults.NotFound(authorId);
        }
        _context.Authors.Remove(entity);
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

}
