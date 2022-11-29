using Microsoft.AspNetCore.Http.HttpResults;
using NSubstitute.ReturnsExtensions;
using ProjectBravo.Core;
using System.Globalization;
using System.Xml.Linq;

namespace ProjectBravo.Api.Tests;

public class GitRepoControllerTests
{
    private readonly IGitRepoRepository _substituteRepo;
    private readonly ICommitRepository _subtituteCommitRepo;

    private readonly GitRepoController _sut;

    private readonly IGitHelper _helper;
    private readonly IFreshGitHelper _fresh;
    private readonly IClonedGitHelper _cloned;

    private readonly IFinalGitHelper _final;


    public GitRepoControllerTests()
    {
        _substituteRepo = Substitute.For<IGitRepoRepository>();
        _subtituteCommitRepo = Substitute.For<ICommitRepository>();

        _sut = new GitRepoController(_substituteRepo, _subtituteCommitRepo);

        _helper = Substitute.For<IGitHelper>();
        _fresh = Substitute.For<IFreshGitHelper>();
        _cloned = Substitute.For<IClonedGitHelper>();
        _final = Substitute.For<IFinalGitHelper>();
    }

    [Fact]
    public async Task GetFrequency_given_useen_repo()
    {
        // Arrange
        var dateForClonedRepo = DateTime.Now.AddDays(-2);

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.ThenAddNewDbEntry().Returns(_final);

        _final.ThenReturnFrequencyString()
        .Returns(Task.FromResult($"1 {dateForClonedRepo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetFrequency("Billy",1, "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"1 {dateForClonedRepo.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }

    [Fact]
    public async Task GetFrequency_given_already_seen_repo_same_version()
    {
        // Arrange
        var dateForCurrentVersion = DateTime.Now.AddDays(-2);
        var billyschat = new GitRepository
        {
            Id = 1,
            Name = "Billy's-chittychat",
            LatestCommitDate = dateForCurrentVersion,
            Authors = new HashSet<Author> { new Author { Name = "Billy", Email = "Billy@mail.com" } },
            Commits = new HashSet<Commit>(),
        };

        //new GitRepositoryDTO(1, "Billy's-chittychat",
        //        dateForCurrentVersion,
        //        new List<string>() { "Billy" },
        //        new List<int>() { 2 }
        //        );

        var result = await _substituteRepo.FindAsync(1);
        var found = result.Result as Ok<GitRepository>;
        found!.Value.Should().Be(billyschat);
            

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.IsNewerThanInDb(billyschat).Returns(false);
        _cloned.ThenGetCurrentFromDb().Returns(_final);

        _final.ThenReturnFrequencyString()
        .Returns(Task.FromResult($"1 {dateForCurrentVersion.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetFrequency("Billy", 1, "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"1 {dateForCurrentVersion.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }

    [Fact]
    public async Task GetFrequency_given_already_seen_repo_newer_version()
    {
        // Arrange
        var dateForCurrentVersion = DateTime.Now.AddDays(-2);

        var newBillyCommit = DateTime.Now;
        var billyschat = new GitRepository
            {
            Id = 1,
            Name = "Billy's-chittychat",
            LatestCommitDate = dateForCurrentVersion,
            Authors = new HashSet<Author> { new Author { Name = "Billy", Email = "Billy@mail.com" } },
            Commits = new HashSet<Commit>(),
        };

        //var result = await _substituteRepo.FindAsync(1);
        //var found = result.Result as Ok<GitRepository>;
        //    .Returns(found.Value);

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.IsNewerThanInDb(billyschat).Returns(true);
        _cloned.ThenUpdateExistingDbEntry().Returns(_final);

        _final.ThenReturnFrequencyString()
        .Returns(Task.FromResult($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetFrequency("Billy", 1, "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }

    [Fact]
    public async Task GetAuthor_given_unseen_repo()
    {
        // Arrange 
        var newBillyCommit = DateTime.Now;
        var billyschat = new GitRepository
        {
            Id = 1,
            Name = "Billy's-chittychat",
            LatestCommitDate = newBillyCommit,
            Authors = new HashSet<Author> { new Author { Name = "Billy", Email = "Billy@mail.com" } },
            Commits = new HashSet<Commit>(),
        };

        _substituteRepo.FindAsync(1).Result.Should().BeNull();
            ;

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.ThenAddNewDbEntry().Returns(_final);

        _final.ThenReturnAuthorString()
        .Returns(Task.FromResult($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetAuthor("Billy", 1,  "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }

    [Fact]
    public async Task GetAuthor_given_already_seen_repo_same_version()
    {
        // Arrange 
        var newBillyCommit = DateTime.Now;
        var billyschat = new GitRepository
        {
            Id = 1,
            Name = "Billy's-chittychat",
            LatestCommitDate = newBillyCommit,
            Authors = new HashSet<Author> { new Author { Name = "Billy", Email = "Billy@mail.com" } },
            Commits = new HashSet<Commit>(),
        };

        //_substituteRepo.FindAsync("Billy's-chat")
        //    .Returns(billyschat);

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.IsNewerThanInDb(billyschat).Returns(false);
        _cloned.ThenGetCurrentFromDb().Returns(_final);

        _final.ThenReturnAuthorString()
        .Returns(Task.FromResult($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetAuthor("Billy", 1, "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"1 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
        res.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAuthor_given_already_seen_repo_newer_version()
    {
        // Arrange 
        var previousDate = DateTime.Now.AddDays(-2);

        var newBillyCommit = DateTime.Now;
        var billyschat = new GitRepository
        {
            Id = 1,
            Name = "Billy's-chittychat",
            LatestCommitDate = newBillyCommit,
            Authors = new HashSet<Author> { new Author { Name = "Billy", Email = "Billy@mail.com" } },
            Commits = new HashSet<Commit>(),
        };

        //_substituteRepo.FindAsync("Billy's-chat")
        //    .Returns(billyschat);

        _helper.CreateInstance(Arg.Any<IGitRepoRepository>(), Arg.Any<ICommitRepository>())
                .Returns(_fresh);

        _fresh.ThenCloneGitRepository("Billy", "Billy's-chat")
            .Returns(_cloned);

        _cloned.IsNewerThanInDb(billyschat).Returns(true);
        _cloned.ThenUpdateExistingDbEntry().Returns(_final);

        _final.ThenReturnAuthorString()
        .Returns(Task.FromResult($"2 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"));

        // Act
        var res = await _sut.GetAuthor("Billy", 1, "Billy's-chat", _helper);

        // Assert
        res.Should().Be($"2 {newBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");
    }
}