using System.Reflection;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ProjectBravo.Tests;

public sealed class ProgramTests
{
    private readonly string _gitRepoPath = @"../../../git-test-repos/git";

    [Fact]
    public void Program_given_commit_frequency()
    {
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        var program = Assembly.Load(nameof(ProjectBravo));

        program.EntryPoint?.Invoke(null, new[] { new string[] { _gitRepoPath, "frequency" } });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        var expected = 
            @"8 2022-09-21
2 2022-09-18
8 2022-09-17
3 2022-09-16
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

        program.EntryPoint?.Invoke(null, new[] { new string[] { _gitRepoPath, "author" } });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();
        var expected = 
            "emjakobsen1\r\n\t6 2022-09-21\r\n\t1 2022-09-18\r\n\t7 2022-09-17\r\n\r\nrakulmaria\r\n\t1 2022-09-21\r\n\r\nGustav\r\n\t1 2022-09-21\r\n\r\nrakul\r\n\t1 2022-09-18\r\n\t1 2022-09-17\r\n\r\nHelgeCPH\r\n\t2 2022-09-16\r\n\t1 2022-09-15\r\n\r\nRasmus Lystrøm\r\n\t1 2022-09-16\r\n\t1 2022-09-15\r\n\t1 2022-09-14\r\n\t2 2021-09-16\r\n\r\nPaolo Tell\r\n\t5 2021-09-17";

        Assert.Equal(expected, output);
    }
}
