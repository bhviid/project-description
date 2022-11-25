using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit.Sdk;

namespace ProjectBravo.Infrastructure.Tests;

public class GitRepoRepositoryTests
{
    private readonly IGitRepoRepository _repo;
    private readonly GitContext _context;

    public GitRepoRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<GitContext>()
            .UseSqlite(connection)
            .Options;

        var context = new GitContext(options);

        _context = context;
        _repo = new GitRepoRepository(_context, new GitRepositoryValidator());
        _context.Database.EnsureCreated();
        var newRepo = new GitRepositoryEntity
        {
            Name = "seed repo",
            Authors = new HashSet<Author> { new Author { Email = "frederik@gmail.com", Name = "frederik" } },
            Commits = new HashSet<Commit> { new Commit { Author = new Author { Name = "Frederik" }, Sha = "unique sha", Date = DateTime.MinValue, RepoName = "a new repo", Message = "a message" } },
            LatestCommitDate = DateTime.MinValue,
        };
        _context.Repos.Add(newRepo);
        _context.SaveChanges();
            
        
    }

    [Fact]
    public async Task CreateAsync_returns_Created_and_frederik()
    {
        // Arrange
        var repo = new GitRepository
        {
            Name = "anewrepo",
            Authors = new HashSet<Author> { new Author { Name = "frederik", Email = "Frederik@gmail.com" } },
            Commits = new HashSet<Commit> { new Commit { Author = new Author { Name = "Joe" }, Sha = "another unique sha", Date = DateTime.MinValue, RepoName = "arepo", Message = "a message" } },
            LatestCommitDate = DateTime.MinValue,

        };
        var result = await _repo.CreateAsync(repo);
        var created = result.Result as Created<GitRepository>;
        created!.Value!.Name.Should().Be("anewrepo");

    }

    [Fact]
    public async Task Find_given_existing_repo_name_returns_repo()
    {
        // Arrange
        var result = await _repo.FindAsync(1);
        var found = result.Result as Ok<GitRepository>;
        found!.Value!.Name.Should().Be("seed repo");
    }

    //    [Fact]
    //    public async Task Find_given_non_existing_repo_name_returns_null()
    //    {
    //        // Arrange

    //        // Act
    //        var found = await _repo.FindAsync("Etrepo");

    //        // Assert
    //        found.Should().BeNull();
    //    }

    //    [Fact]
    //    public async Task Update_given_existing_succeeds()
    //    {
    //        // Arrange
    //        _context.Repos.Add(new GitRepository
    //        {
    //            Name = "Etrepo",
    //            Authors = new HashSet<Author> { new Author { Name = "Frederik" } },
    //            Commits = new HashSet<Commit> { new Commit { Message = "HVAS�", Date = DateTime.Now.AddHours(-5), Author = new Author { Name = "asger" } } },
    //            LatestCommitDate = DateTime.Now,
    //        });
    //        var newCommit = new Commit()
    //        {
    //            Message = "New commit",
    //            Date = DateTime.Now,
    //            Author = new Author("Billy")

    //        };
    //        _context.Commits.Add(newCommit);
    //        _context.SaveChanges();


    //        var toUpdateDto = new GitRepositryUpdateDTO(
    //            "Etrepo",
    //            new List<string>() { newCommit.Author.Name },
    //            new List<int>() { newCommit.Id });

    //        // Act
    //        var updated = await _repo.UpdateAsync(toUpdateDto);


    //        // Assert
    //        updated.CommitIds.Should().Contain(newCommit.Id);
    //        updated.CommitIds.Should().HaveCount(2);
    //    }

    //    //[Fact] what should it do?
    //    public async Task Update_given_not_existing_repo()
    //    {
    //        // Arrange
    //        var toUpdateDto = new GitRepositryUpdateDTO(
    //            "Etrepo",
    //            new List<string>() { "Bob" },
    //            new List<int>() { 1, 2 });

    //        // Act

    //        var hmm = await _repo.UpdateAsync(toUpdateDto);

    //        // Assert

    //    }
}