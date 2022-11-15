using System.Globalization;
using System.IO.Compression;
using FluentAssertions;

namespace ProjectBravo.Tests;

public class GitHelperTests : IDisposable
{
    private readonly string _pathToGitString;
    private readonly GitInsights _sut;
    public GitInsightsTests()
    {
        IGitRepoRepository repo = new GitRepository();

        
    }

    /* public static IFreshGitHelper CreateInstance(IGitRepoRepository gitRepoDbRepo, ICommitRepository gitCommitDbRepo)
    {
        return new GitHelper(gitRepoDbRepo, gitCommitDbRepo);
    }*/
}