using System.Reflection;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ProjectBravo.Tests;

public sealed class ProgramTests
{
    private readonly string _gitRepoPath = @"C:\Users\tilsi\OneDrive\Skrivebord\project-description";

    [Fact]
    public void Program_given_commit_frequency()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        var program = Assembly.Load(nameof(ProjectBravo));

        program.EntryPoint?.Invoke(null, 
                        new[] { new string[] { _gitRepoPath, "frequency" } });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        var expected = @"9 2022-09-20
1 2022-09-19
4 2022-09-16
2 2022-09-15
1 2022-09-14
5 2021-09-17
2 2021-09-16";

        Assert.Equal(expected, output);
    }  

    [Fact]
    public void Program_given_commit_author_frequency()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        var program = Assembly.Load(nameof(ProjectBravo));

        program.EntryPoint?.Invoke(null, 
                        new[] { new string[] { _gitRepoPath, "author" } });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        var expected = @"";

        Assert.Equal(expected, output);
    }
}