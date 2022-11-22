using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectBravo.Core;

public interface IAuthorRepository
{
    Task<Results<Created<Author>, ValidationProblem>> CreateAsync(AuthorCreateDTO author);
    Task<Results<Ok<Author>, NotFound<int>>> FindAsync(int authorId);
    Task<IReadOnlyCollection<Author>> ReadAsync();

    Task<Results<NoContent, NotFound<int>>> DeleteAsync(int authorId);
    Task<Results<NoContent, NotFound<int>>> UpdateAsync(int id, Author author);
}