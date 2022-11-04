using System.Globalization;
using System.IO.Compression;
using FluentAssertions;
using static ProjectBravo.GitInsights;

namespace ProjectBravo.Tests;

public class GitInsightsTests : IDisposable
{
    private readonly string _pathToGitString;
    public GitInsightsTests()
    {
        var pathToGit = new FileInfo(@"../git-test-repos/").Directory.FullName;
        ZipFile.Open(@"../../../git-test-repos/git.zip", 0).ExtractToDirectory(@"../git-test-repos/");
        _pathToGitString = pathToGit + @"/git";
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
        var output = GenerateCommitsByDate(_pathToGitString);

        // Assert
        output.Should().Contain(x => x.Count() == expectedAmount 
                && x.Key.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) == date);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void GenerateCommitsByAuthor_git_folder_success(string author, (int,string)[] commitNumToDate)
    {
        // Arrange

        // Act
        var output = GenerateCommitsByAuthor(_pathToGitString);

        // Assert
        output.Keys.Should().Contain(author);
    }

    public static IEnumerable<object[]> Data()
    {
        yield return new object[] { "emjakobsen1", new[] { (6, "2022-09-21"), (1, "2022-09-18"), (7, "2022-09-17") } };
        yield return new object[] { "rakulmaria", new[] { (1, "2022-09-21") } };
        yield return new object[] { "Rasmus Lystrøm", new[] {(1, "2022-09-16"), (1, "2022-09-15"), (1, "2022-09-14"), (2, "2021-09-16")} };
        yield return new object[] { "Paolo Tell", new[] { (5, "2021-09-17")} };
        yield return new object[] { "HelgeCPH", new[]{ (2, "2022-09-16"), (1, "2022-09-15") } };
    }

    public void Dispose()
    {
        Directory.Delete(new FileInfo(@"../git-test-repos/").Directory.FullName, true);
    }
}