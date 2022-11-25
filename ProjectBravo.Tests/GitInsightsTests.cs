using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using FluentAssertions;
using ProjectBravo.Core;
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

    /// <summary>
    /// The following 2 tests will only pass after the following setup
    /// In the ProjectBravo Project execute command: "dotnet user-secrets set token <YOUR TOKEN>"
    /// </summary>
    [Theory]
    [InlineData("silasarildsen", "test")]
    public async Task GetRepoForks_returns_correct_data(string owner, string repo)
    {
        // Arrange
        var expected = new ForkDTO(
                Id: 567672216,
                Name: "test",
                AuthorId: 43112458,
                AuthorName: "silasarildsen1"
            );

        // Act
        var res = await _sut.GetRepoForks(owner, repo);
        var actual = res[3];

        // Assert
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData("silasarildsen", "test")]
    public async Task GetRepoForks_returns_right_number_of_forks(string owner, string repo)
    {
        // Arrange
        var expected = 4;

        // Act
        var res = await _sut.GetRepoForks(owner, repo);
        var actual = res.Count;

        // Assert
        actual.Should().Be(expected);
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