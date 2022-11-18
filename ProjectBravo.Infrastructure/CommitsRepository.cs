using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBravo.Infrastructure;

public class CommitsRepository : ICommitRepository
{
    private readonly GitContext _context;
    
    public CommitsRepository(GitContext context)
    {
        _context = context;
        
    }
    public async Task<(Status, CommitDTO)> CreateAsync(CommitCreateDTO commit)
    {
        var entity = new Commit
        {
            Date = commit.Date,
            RepositoryId = commit.RepositoryId,
            Author = new Author(commit.AuthorName, commit.Email),
            Message = commit.Message,
        };
        var exists = await _context.Commits.FirstOrDefaultAsync(c => c.Message == entity.Message);

        if (exists == null)
        {
            await _context.Commits.AddAsync(entity);
            await _context.SaveChangesAsync();

            CommitDTO dto = new CommitDTO(entity.Id, entity.Date, entity.Message, entity.Author.Name, entity.RepositoryId);
            return (Created, dto);
        }
        else return (Conflict, null);
       




    }

    public async Task<(Status, CommitDTO?)> FindAsync(int commitId)
    {
        var entity = from c in _context.Commits
                     where c.Id == commitId
                     select c;
        var dto = await entity.FirstOrDefaultAsync();
        if (dto != null)
        {
            CommitDTO dtoToBeReturned = new CommitDTO(dto.Id, dto.Date, dto.Message, dto.Author.Name, dto.RepositoryId);
            return (OK, dtoToBeReturned);
        }
        else return (Status.NotFound, null);
    }

                        

    public async Task<IReadOnlyCollection<CommitDTO>> ReadAsync()
    {
        var entities = from c in _context.Commits
                       where c != null
                       select new CommitDTO(
                           c.Id,
                           c.Date,
                           c.Message,
                           c.Author.Name,
                           c.RepositoryId
                           );

        return await entities.ToListAsync();
    }
}
