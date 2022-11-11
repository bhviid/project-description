using LibGit2Sharp;
using NSubstitute.ReturnsExtensions;
using System.Globalization;
using System.IO.Compression;

namespace ProjectBravo.Api.Tests;

public class GitRepoControllerTests : IDisposable
{
    private readonly IGitRepoRepository _substituteRepo;
    private readonly IGitAnalyzer _analyzer;
    private readonly GitRepoController _sut;

    private readonly string _pathToGitString;

    public GitRepoControllerTests()
    {
        _substituteRepo = Substitute.For<IGitRepoRepository>();
        _analyzer = Substitute.For<IGitAnalyzer>();

        _sut = new GitRepoController(_substituteRepo, _analyzer);

        string newF = @"../../../git-test-repos-no-zip/";

        var pathToGit = new FileInfo(@"../git-test-repos/").Directory.FullName;
        ZipFile.Open(@"../../../git-test-repos/git.zip", 0).ExtractToDirectory(newF);
        _pathToGitString = newF + @"/git/bob/.git/";
    }

    public void Dispose()
    {
        var dir = new System.IO.DirectoryInfo(@"../../../git-test-repos-no-zip/");
        if (dir.Exists)
        {
            setAttributesNormal(dir);
            dir.Delete(true);
        }
    }

    private void setAttributesNormal(DirectoryInfo dir)
    {
        foreach (var subDir in dir.GetDirectories())
        setAttributesNormal(subDir);
        foreach (var file in dir.GetFiles())
        {
            file.Attributes = FileAttributes.Normal;
        }
    }

    [Fact]
    public async Task Frequency_given_fresh_or_unseen_repo()
    {
        // Arrange
        var newClonedRepo = new Repository(_pathToGitString);

        var author = new Signature("Billy", "Billy@example.com", DateTime.Now);

        var commit = newClonedRepo.Commit("I commited", author, author);

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

    [Fact]    
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