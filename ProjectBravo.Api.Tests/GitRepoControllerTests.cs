using LibGit2Sharp;
using NSubstitute.ReturnsExtensions;
using System.Globalization;

namespace ProjectBravo.Api.Tests;

public class GitRepoControllerTests
{
    private readonly IGitRepoRepository _substituteRepo;
    private readonly IGitAnalyzer _analyzer;
    private readonly GitRepoController _sut;

    public GitRepoControllerTests()
    {
        //Maybe we want to substitute the GitInsights class to?
        // that way we can actually control what the controller does?

        // We would wanna make an interface for the gitinsights, then make GitInsights an 
            //instantiatable class.

        _substituteRepo = Substitute.For<IGitRepoRepository>();
        _analyzer = Substitute.For<IGitAnalyzer>();

        _sut = new GitRepoController(_substituteRepo);
    }

    [Fact]
    public async Task Frequency_given_fresh_or_unseen_repo()
    {
        // Arrange
        var newClonedRepo = new Repository();
        newClonedRepo.Commit("Yoylo",
            new Signature("Billy", "Billy@example.com", DateTime.Now),
            new Signature("Billy", "Billy@example.com", DateTime.Now),
            new CommitOptions()
            );

        _substituteRepo.FindAsync(Arg.Any<string>()).ReturnsNull();

        _analyzer.CloneGithubRepo(Arg.Any<string>(), Arg.Any<string>())
            .Returns(newClonedRepo);

        // hmm
        _analyzer.GetFrequencyString(Arg.Is(newClonedRepo))
            .Returns($"1 {DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

        // Act
        var res = await _sut.GetFrequency("bhviid", "project-description");

        // Assert
        var expected = $"1 {DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

        res.TrimEnd().Should().Be(expected);
    }

    public async Task Frequency_given_already_seen_repo_same_version()
    {
        // Arrange
        var dateForBillyCommit = DateTime.Now.AddDays(-2);

        var repoInDb = new GitRepositoryDTO(1, "Billy's-chittychat",
            dateForBillyCommit,
            new List<string>() { "Billy" },
            new List<int>() { 2 }
            );

        var clonedRepo = new Repository();
        clonedRepo.Commit("Yoylo",
            new Signature("Billy", "Billy@example.com", dateForBillyCommit),
            new Signature("Billy", "Billy@example.com", dateForBillyCommit),
            new CommitOptions()
        );

        _substituteRepo.FindAsync(Arg.Any<string>()).Returns(repoInDb);
        _analyzer.CloneGithubRepo(Arg.Any<string>(), Arg.Any<string>()).Returns(clonedRepo);

        _analyzer.GetFrequencyString(Arg.Is(clonedRepo))
            .Returns($"1 {dateForBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}");

        // Act
        var res = await _sut.GetFrequency("Billy", "Billy's-chittychat");

        // Assert
        var expected = $"1 {dateForBillyCommit.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";
        
        res.TrimEnd().Should().Be(expected);
    }

    public async Task Frequency_given_already_seen_repo_newer_version()
    {
        // Arrange

        // Act

        // Assert
    }
}