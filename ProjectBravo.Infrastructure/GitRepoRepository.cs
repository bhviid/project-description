using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectBravo.Infrastructure;

namespace ProjectBravo.Infrastructure;

public class GitRepoRepository : IGitRepoRepository
{
    private readonly GitContext _context;

    public GitRepoRepository(GitContext context)
    {
        _context = context;
    }

    public async Task<(Status, GitRepositoryDTO)> CreateAsync(GitRepositryCreateDTO gitRepo)
    {
        var entity = await _context.Repos.FirstOrDefaultAsync(git => git.Name == gitRepo.Name);
        Status status;

        if (entity is null)
        {

            var repo = new GitRepository
            {
                Name = gitRepo.Name,
                Authors = await CreateOrUpdateAuthor(gitRepo.Authors),
                Commits = await CreateOrUpdateCommit(gitRepo.Commits),
                LatestCommitDate = GetLatestCommit(gitRepo.Commits)
            };



            _context.Repos.Add(repo);
            await _context.SaveChangesAsync();

            status = Created;
            return (status, new GitRepositoryDTO(repo.Id, gitRepo.Name, DateTime.Now, authorDtoToAuthorCreateDto(gitRepo.Authors).ToList(),  gitRepo.Commits));
        }
        else
        {
            status = Conflict;
        }



        var created = new GitRepositoryDTO(entity.Id, entity.Name, entity.LatestCommitDate, toAuthorDtos(entity.Authors), toCommitCreateDtos(entity.Commits));



        return (status, created);
    }


    public async Task<(Status, GitRepositoryDTO?)> FindAsync(string gitRepoName)
    {
        var repos = from r in _context.Repos
                    where r.Name == gitRepoName
                    select new GitRepositoryDTO(r.Id, r.Name, r.LatestCommitDate, toAuthorDtos(r.Authors), toCommitCreateDtos(r.Commits));
        if (await repos.FirstOrDefaultAsync() == null)
        {

            return (NotFound, null);
        }
        else
        {

            return (Status.OK, await repos.FirstOrDefaultAsync());
        }
    }

    public async Task<IReadOnlyCollection<GitRepositoryDTO>> ReadAsync()
    {
        var repos = from r in _context.Repos
                    orderby r.Name
                    select new GitRepositoryDTO(r.Id, r.Name, r.LatestCommitDate, toAuthorDtos(r.Authors), r.Commits);
        return await repos.ToListAsync();
    }
    //todo update commits
    //
    public async Task<Status>UpdateAsync(GitRepositryUpdateDTO gitRepo)
    {
        var entity = await _context.Repos.FindAsync(gitRepo.Name);
        Status status;
        if (entity is null)
        {
            status = NotFound;
        }
        //else if (await _context.Repos.FirstOrDefaultAsync(r => r.Id != gitRepo.Id && r.Name == gitRepo.Name) != null)
        //{
        //    status = Conflict;
        //}
        else
        {
            entity.Name = gitRepo.Name;
            var authorNameList = gitRepo.Authors;
            var authorSet = new HashSet<Author>();
            foreach (var author in authorNameList)
            {
                var a = new Author(author.Name, author.Email);
                authorSet.Add(a);

            }
            entity.Authors = authorSet;

            await _context.SaveChangesAsync();
            status = Updated;

        }
        return status;
    }

    public async Task<Status> DeleteAsync(int gitRepoid)
    {
        var gitRepo = await _context.Repos.Include(r => r.Authors).FirstOrDefaultAsync(r => r.Id == gitRepoid);
        Status status;
        if (gitRepo is null)
        {
            status = NotFound;

        }
        else if (gitRepo.Authors.Any())
        {
            status = Conflict;
        }
        else
        {
            _context.Repos.Remove(gitRepo);
            await _context.SaveChangesAsync();
            status = Deleted;

        }
        return status;

        
    }
    private List<AuthorDTO> toAuthorDtos(HashSet<Author> set)
    {
        var AuthorList = new List<AuthorDTO>();
        foreach (var author in set)
        {
            var temp = new AuthorDTO(author.Id, author.Name, author.Email);
            AuthorList.Add(temp);
        }
        return AuthorList;
    }

   
    private async Task<HashSet<Author>> CreateOrUpdateAuthor(IEnumerable <AuthorCreateDTO> list)
    {
        var existing = _context.Authors.Where(a => list.Any(l => l.Email == a.Email)).ToDictionary(a => a.Email);
        var AuthorSet = new HashSet<Author>();
        foreach (var item in list)
        {
            existing.TryGetValue(item.Email, out var author);

            AuthorSet.Add(author ?? new Author(item.Name, item.Email));
            
        }
        return AuthorSet;
    }

    private async Task<HashSet<Commit>> CreateOrUpdateCommit(IEnumerable<CommitCreateDTO> list)
    {
        var existing = _context.Commits.Where(c => list.Any(l => l.Sha == c.Sha)).ToDictionary(a => a.Sha);
        var CommitSet = new HashSet<Commit>();
        foreach (var item in list)
        {
            existing.TryGetValue(item.Sha, out var commit);

            CommitSet.Add(commit ?? new Commit(item.RepositoryId, new Author(item.AuthorName, item.Email), item.Date, item.Message, item.RepoName, item.Sha;

        }
        return CommitSet;
    }

    private DateTime GetLatestCommit(IEnumerable<CommitCreateDTO> commits)
    {
        DateTime LatestDateTime = DateTime.MinValue;
        foreach (var commit in commits)
        {
            if (commit.Date > LatestDateTime) { LatestDateTime= commit.Date; }
        }
        return LatestDateTime;
    }
}

