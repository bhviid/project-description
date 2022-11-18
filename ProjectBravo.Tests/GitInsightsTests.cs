using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using FluentAssertions;
using ProjectBravo.Infrastructure;

namespace ProjectBravo.Tests;

public class GitInsightsTests : IDisposable
{
    private readonly string _pathToGitString;
    private readonly GitInsights _sut;
    public GitInsightsTests()
    {
        var pathToGit = new FileInfo(@"../git-test-repos/").Directory.FullName;
        ZipFile.Open(@"../../../git-test-repos/git.zip", 0).ExtractToDirectory(@"../git-test-repos/");
        _pathToGitString = pathToGit + @"/git";

        _sut = new GitInsights();
    }

    [Theory]
    [InlineData(8, "2022-09-21")]
    [InlineData(2, "2022-09-18")]
    [InlineData(8, "2022-09-17")]
    [InlineData(3, "2022-09-16")]
    [InlineData(2, "2022-09-15")]

    public void GenerateCommitsByDate_git_folder(int expectedAmount, string date)
    {
        // Arrange

        // Act
        var output = _sut.GenerateCommitsByDate(_pathToGitString);

        // Assert
        output.Should().Contain(x => x.Count() == expectedAmount
                && x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == date);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GenerateCommitsByAuthor_git_folder_success(string author, (int, string)[] commitNumToDate)
    {
        // Arrange

        // Act
        var output = _sut.GenerateCommitsByAuthor(_pathToGitString);

        // Does the test require its own test, hmm
        var outputNumToDate = output[author].GroupBy(c => c.Author.When.Date)
            .Select(x => (x.Count(), x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

        // Assert
        output.Keys.Should().Contain(author);
        outputNumToDate.Should().BeEquivalentTo(commitNumToDate);

    }

    [Theory]
    [InlineData("SilasArildsen", "test")]
    public void Get_forks(string owner, string repo)
    {
        // Arrange
        var parent = new GitRepository();
        parent.Id = 0;
        parent.Name = repo;

        var parentAuthor = new Author();
        parentAuthor.Id = 0;
        parentAuthor.Name = owner;
        parent.Authors = new HashSet<Author>() { parentAuthor };
        

        var fork = new Fork();
        fork.Id = 1;
        fork.Name = "test";

        var forkAuthor = new Author();
        forkAuthor.Name = "silasarildsen1";
        forkAuthor.Id = 1;
        fork.Authors = new HashSet<Author>() { forkAuthor };

        fork.Parent = parent;

        var expected = new List<Fork>() { fork };

        // Act
        List<Fork> actual = _sut.GetRepoForks(owner, repo);

        // Assert
        actual.Should().BeEquivalentTo(expected);

    }

    public static IEnumerable<object[]> Data()
    {
        yield return new object[] { "emjakobsen1", new[] { (6, "2022-09-21"), (1, "2022-09-18"), (7, "2022-09-17") } };
        yield return new object[] { "rakulmaria", new[] { (1, "2022-09-21") } };
        yield return new object[] { "Rasmus Lystrøm", new[] { (1, "2022-09-16"), (1, "2022-09-15"), (1, "2022-09-14"), (2, "2021-09-16") } };
        yield return new object[] { "Paolo Tell", new[] { (5, "2021-09-17") } };
        yield return new object[] { "HelgeCPH", new[] { (2, "2022-09-16"), (1, "2022-09-15") } };
    }

    public void Dispose()
    {
        Directory.Delete(new FileInfo(@"../git-test-repos/").Directory.FullName, true);
    }
}