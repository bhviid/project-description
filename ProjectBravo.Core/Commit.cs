using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ProjectBravo.Core;

public sealed record Commit
{
    public int Id { get; set; }

    public int RepositoryId { get; init; }

    public Author Author { get; init; }

    public DateTime Date { get; set; }

    public string Message { get; set; }
    public string RepoName { get; set; }

    public string Sha { get; set; }

}
public class CommitValidator : AbstractValidator<Commit>
{
    public CommitValidator()
    {
        RuleFor(c => c.Author).NotEmpty();
        RuleFor(c => c.RepositoryId).NotEmpty();
        RuleFor(c => c.RepoName).NotEmpty();
        RuleFor(c => c.Sha).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();

    }
}
