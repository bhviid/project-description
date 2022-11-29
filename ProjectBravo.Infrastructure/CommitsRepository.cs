
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectBravo.Core;

namespace ProjectBravo.Infrastructure;

public class CommitsRepository : ICommitRepository
{
    private readonly GitContext _context;
    private readonly CommitValidator _validator;
    
    
    public CommitsRepository(GitContext context, CommitValidator commitValidator)
    {
        _context = context;
        _validator = commitValidator;
    }

    public async Task<Results<Created<Commit>, ValidationProblem>> CreateAsync(Commit commit)
    {
        var validation = _validator.Validate(commit);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var entity = new CommitEntity{RepositoryId = commit.RepositoryId,
                                        Author =  await CreateOrUpdateAuthor(commit.Author),
                                        Date = commit.Date,
                                        Message = commit.Message,
                                        RepoName = commit.RepoName,
                                        Sha = commit.Sha };
                                        
        await _context.Commits.AddAsync(entity);
        await _context.SaveChangesAsync();

        return TypedResults.Created($"{entity.Id}", commit with { Id = entity.Id});
    }

    public async Task<Results<Ok<Commit>, NotFound<int>>> FindAsync(int commitId)
    {
        var commits = from c in _context.Commits
                     where c.Id == commitId

                     select new Commit {
                         Id = c.Id,
                         RepositoryId = c.RepositoryId,
                         Author = new Author { Id= c.Author.Id , Email = c.Author.Email, Name = c.Author.Name},
                         Date = c.Date,
                         Message = c.Message,
                         RepoName = c.RepoName,
                         Sha = c.Sha

                         };
        var commit = await commits.FirstOrDefaultAsync();
        return commit is null ? TypedResults.NotFound(commitId) : TypedResults.Ok(commit); 
    }              

    public async Task<IReadOnlyCollection<Commit>> ReadAsync()
    {
        var entities = from c in _context.Commits
                       where c != null
                       select new Commit
                       {
                           Id = c.Id,
                           RepositoryId = c.RepositoryId,
                           Author = new Author { Id = c.Author.Id, Email = c.Author.Email, Name = c.Author.Name },
                           Date = c.Date,
                           Message = c.Message,
                           RepoName = c.RepoName,
                           Sha = c.Sha
                       };

        return await entities.ToListAsync();
    }

    private async Task<AuthorEntity?> CreateOrUpdateAuthor(Author author)
    {
        var existingAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.Email == author.Email);
        if (existingAuthor == null)
        {
            var newAuthor = new AuthorEntity(author.Name, author.Email);
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();
            return newAuthor;
        }
        else return existingAuthor;
        
        
    }
}
