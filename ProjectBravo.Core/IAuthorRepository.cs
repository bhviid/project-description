using Microsoft.AspNetCore.Http.HttpResults;

namespace ProjectBravo.Core;

public interface IAuthorRepository
{
    public Task<Results<Created<Author>, ValidationProblem>> CreateAsync(Author author);
    public Task<Results<Ok<Author>, NotFound<int>>> FindAsync(int authorId);
    public Task<IReadOnlyCollection<Author>> ReadAsync();

    public Task<Results<NoContent, NotFound<int>>> DeleteAsync(int authorId);
    public Task<Results<NoContent, NotFound<int>>> UpdateAsync(int id, Author author);
}