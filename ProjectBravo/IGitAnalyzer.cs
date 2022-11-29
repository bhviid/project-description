using LibGit2Sharp;
using ProjectBravo.Core;

namespace ProjectBravo;

public interface IGitAnalyzer
{
    string GetFrequencyString(Repository repo);

    string GetFrequencyString(IEnumerable<Core.Commit> commits);

    string GetAuthorString(Repository repo);

    // Man kunne endda spørge sig sevl om en GitRepositoryDTO ikke er rigeligt at returnere.
        // Men kunne evt. lade GitRepositoryDTO holde en liste af CommitDTO.
        // På den måde slipper api fuldstændigt uden om Repo, og vi bruger vores onion-struktur.
    Repository CloneGithubRepo(string user, string repoName);
}
