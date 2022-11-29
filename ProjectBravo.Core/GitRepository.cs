using FluentValidation;

namespace ProjectBravo.Core;

public sealed record GitRepository
{
    public int Id { get; set; }

    public string Name { get; set; }

    public HashSet<Author> Authors { get; set; }

    public HashSet<Commit> Commits { get; set; }

    public DateTime LatestCommitDate { get; set; }
}

public class GitRepositoryValidator : AbstractValidator<GitRepository>
{
    public GitRepositoryValidator()
    {
        
    //Insert rules

    }
}