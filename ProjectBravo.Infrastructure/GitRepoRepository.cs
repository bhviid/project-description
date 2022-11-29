using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectBravo.Core;
using ProjectBravo.Infrastructure;

namespace ProjectBravo.Infrastructure;

public class GitRepoRepository : IGitRepoRepository
{
    private readonly GitContext _context;
    private readonly GitRepositoryValidator _validator;
    


    public GitRepoRepository(GitContext context, GitRepositoryValidator validator)
    {
        _context = context;
        _validator = validator;
        
    }

    public async Task<Results<Created<GitRepository>, ValidationProblem>> CreateAsync(GitRepository gitRepository)
    {
        var validation = _validator.Validate(gitRepository);

        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var exists = _context.Repos.FirstOrDefaultAsync(r => r.Name == gitRepository.Name);

        if (exists != null)
        {
            //Todo
        }
        var entity = new GitRepositoryEntity
        {
            Name = gitRepository.Name,
            Authors = gitRepository.Authors,
            Commits = gitRepository.Commits,
            LatestCommitDate = gitRepository.LatestCommitDate,
        };

        _context.Repos.Add(entity);
        await _context.SaveChangesAsync();
        //return TypedResults.Created($"{entity.Id}", commit with { Id = entity.Id});
        return TypedResults.Created($"{entity.Id}", gitRepository with { Id = entity.Id});


    }


    public async Task<Results<Ok<GitRepository>, NotFound<int>>> FindAsync(int repositoryId)
    {
        var repos = from r in _context.Repos
                    where r.Id == repositoryId
                    select new GitRepository
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Authors = r.Authors,
                        Commits = r.Commits,
                        LatestCommitDate = r.LatestCommitDate,
                    };
        var repo = await repos.FirstOrDefaultAsync();
        return repo is null ? TypedResults.NotFound(repositoryId) : TypedResults.Ok(repo);
    }

    public async Task<IReadOnlyCollection<GitRepository>> ReadAsync()
    {
        var repos = from r in _context.Repos
                    orderby r.Name
                    select new GitRepository
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Authors = r.Authors,
                        Commits = r.Commits,
                        LatestCommitDate = r.LatestCommitDate,
                    };
        return await repos.ToListAsync();
    }
    //todo update commits

    public async Task<Results<NoContent, NotFound<int>>> UpdateAsync(int repoId, GitRepository gitRepository)
    {
        var entity = await _context.Repos.FindAsync(repoId);
       
        if (entity is null)
        {
            return TypedResults.NotFound(repoId);
        }
        entity.Authors = gitRepository.Authors;
        entity.Commits = gitRepository.Commits;
        entity.LatestCommitDate = gitRepository.LatestCommitDate;

        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    public async Task<Results<NoContent, NotFound<int>>> DeleteAsync(int repositoryId)
    {
        var gitRepo = await _context.Repos.FindAsync(repositoryId);
        
        if (gitRepo is null)
        {
            return TypedResults.NotFound(repositoryId);

        }
        _context.Repos.Remove(gitRepo);
        await _context.SaveChangesAsync();

        return TypedResults.NoContent();
    }
    
}

