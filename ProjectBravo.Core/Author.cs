using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ProjectBravo.Core;

public sealed record Author
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}

public  class AuthorValidator : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty();
    }
}

