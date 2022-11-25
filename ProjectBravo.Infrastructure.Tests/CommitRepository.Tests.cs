using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectBravo.Infrastructure.Tests;
public class CommitRepositoryTests
{
    private readonly ICommitRepository _repo;
    private readonly GitContext _context;

    public CommitRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<GitContext>()
            .UseSqlite(connection)
            .Options;

        var context = new GitContext(options);

        _context = context;
        _repo = new CommitsRepository(context, new CommitValidator());
        _context.Database.EnsureCreatedAsync();
        _context.Commits.Add(new CommitEntity {Sha = "unique SHa", RepoName = "seedRepo", RepositoryId = 1, Date = DateTime.MinValue, Author = new AuthorEntity { Email = "mail@mail.com", Name = "name" }, Message = "Yoyoyo", Id = 1 }); ;
        _context.SaveChanges();
    }


    [Fact]
    public async Task CreateAsync_should_return_Frederik()
    {
        //given
        var commit = new Commit { Author = new Author { Name= "Joe" }, Sha = "another unique sha", Date = DateTime.MinValue, RepoName = "arepo" };
        var result = await _repo.CreateAsync(commit);
        
        var created = result.Result as Ok<Created>;

        
    }
    [Fact]
    public async void CreateAsync_should_return_conflict_and_null()
    {
        //given
        var commit = new Commit { Author = new Author { Name = "Joe" }, Sha = "another unique sha", Date = DateTime.MinValue, RepoName = "arepo" };
        var result = await _repo.CreateAsync(commit);
        var validationProblem = result.Result as ValidationProblem;
        validationProblem!.ProblemDetails.Errors.Should().HaveCount(2); //errors are : Repository Id must not be empty, Date must not be empty
    }

    [Fact]
    public async void FindAsync_should_return_Frederik()
    {
        var result = await _repo.FindAsync(1);
        var found = result.Result as Ok<Commit>;
        found!.Value.Should().Be(new Commit { Sha = "unique SHa", RepoName = "seedRepo", RepositoryId = 1, Date = DateTime.MinValue, Author = new Author { Id = 1, Email = "mail@mail.com", Name = "name" }, Message = "Yoyoyo", Id = 1 });


        

    }

    [Fact]
    public async void FindAsync_should_return_notfound_and_null()
    {
        var result = await _repo.FindAsync(42);
        var notFound = result.Result as NotFound<int>;
        notFound!.Value.Should().Be(42);
    }

    [Fact]
    public async void ReadAsync_should_return_count_1()
    {
        var result = await _repo.ReadAsync();
        result.Count.Should().Be(1);
        
    }
}
