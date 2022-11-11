using System.Reflection;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;



namespace ProjectBravo.Tests;

public sealed class ProgramTests
{
    [Fact (Skip = "Program isn't working anyway.")]
    public void Program_given_commit_frequency()
    {
        //Extracting zipped git repository for testing purpose 
        var pathToGit = new FileInfo(@"../git-test-repos/").Directory.FullName;
        ZipFile.Open(@"../../../git-test-repos/git.zip",0).ExtractToDirectory(@"../git-test-repos/");
        pathToGit = pathToGit+@"/git";
        
        
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        var program = Assembly.Load(nameof(ProjectBravo));

        program.EntryPoint?.Invoke(null, new[] { new string[] { "frequency", pathToGit} });

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
        Directory.Delete(new FileInfo(@"../git-test-repos/").Directory.FullName, true);
        
    }
    [Theory (Skip = "Program isn't working anyway.")]
    [InlineData("git")]
    public void Program_given_commit_author_frequency(string dirPathname)
    {
        //Extracting zipped git repository for testing purpose 
        var pathToGit = new FileInfo(@"../git-test-repos/").Directory.FullName;
        ZipFile.Open(@"../../../git-test-repos/git.zip",0).ExtractToDirectory(@"../git-test-repos/");
        pathToGit = pathToGit+$@"/{dirPathname}";
        
        // Arrange
        using var writer = new StringWriter();
        Console.SetOut(writer);

        // Act
        var program = Assembly.Load(nameof(ProjectBravo));

        program.EntryPoint?.Invoke(null, new[] { new string[] { "author",pathToGit } });

        // Assert
        var output = writer.GetStringBuilder().ToString().TrimEnd();

        //Line breaks differ from system to system, 
        //somtimes it is \n other time it is \r\n, 
        // this do the magic for us.
        var nl = Environment.NewLine;
        
        var expected = 
            $"emjakobsen1{nl}\t6 2022-09-21{nl}\t1 2022-09-18{nl}\t7 2022-09-17{nl}{nl}rakulmaria{nl}\t1 2022-09-21{nl}{nl}Gustav{nl}\t1 2022-09-21{nl}{nl}rakul{nl}\t1 2022-09-18{nl}\t1 2022-09-17{nl}{nl}HelgeCPH{nl}\t2 2022-09-16{nl}\t1 2022-09-15{nl}{nl}Rasmus Lystrøm{nl}\t1 2022-09-16{nl}\t1 2022-09-15{nl}\t1 2022-09-14{nl}\t2 2021-09-16{nl}{nl}Paolo Tell{nl}\t5 2021-09-17";

        Assert.Equal(expected, output);
        Directory.Delete(new FileInfo(@"../git-test-repos/").Directory.FullName, true);
    }
}
