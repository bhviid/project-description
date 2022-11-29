using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using ProjectBravo.Infrastructure;
namespace ProjectBravo.Tests;


public class GitHelperTests
{
    private readonly SqliteConnection _connection;
    private readonly GitContext _context;
    private readonly GitRepoRepository _repository;
    private readonly CommitsRepository _commits;
    private readonly string _githubUsername;
    private readonly string _repoName;

    public GitHelperTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
		_connection.Open();
        var builder = new DbContextOptionsBuilder<GitContext>();
        builder.UseSqlite(_connection);
        _context = new GitContext(builder.Options);
        _context.Database.EnsureCreated();

        var testAuthor = new Author { Id = 0, Name = "testAuthor" };
        var testRepo = new GitRepository { Id = 2, Name = "testRepo", Authors = new HashSet<Author> { testAuthor }, Commits = new HashSet<Commit> { }, LatestCommitDate = DateTime.Now };
        var testCommit = new Commit { Id = 1, BelongsTo = testRepo, Author = testAuthor, Message = "testMessage" };
        _githubUsername = "testGithubUser";
        _repoName = "testRepo";

        _context.Repos.Add(testRepo);
        _context.Commits.Add(testCommit);
        _context.Authors.Add(testAuthor);

        _context.SaveChanges();
        _repository = new GitRepoRepository(_context);
        _commits = new CommitsRepository();
    


    }
    // work in progress
    [Fact]
    public async Task Frequency_Should_Match_In_Memomry_Database()
    {
        /*
        var frequency = await GitHelper.CreateInstance(_repository, _commits).ThenCloneGitRepository("bhviid","project-description").ThenAddNewDbEntry().ThenReturnFrequencyString();
        var expected = "Et commit";
        Assert.Equal(expected,frequency);

        */


    }
    
    
}
