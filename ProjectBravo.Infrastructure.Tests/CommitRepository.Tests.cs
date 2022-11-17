using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBravo.Infrastructure.Tests;
public class CommitRepositoryTests
{
    private readonly ICommitRepository _repo;
    private readonly GitContext _context;

    public CommitRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<GitContext>()
            .UseSqlite(connection)
            .Options;

        var context = new GitContext(options);

        _context = context;
        _repo = new CommitsRepository(context); 
        _context.Database.EnsureCreatedAsync();
        _context.Commits.Add(new Commit { BelongsTo = "etrepo", Date = DateTime.Now, Author = new Author("Frederik"), Message = "Yoyoyo", Id = 1 }); ;
        _context.SaveChanges();
    }


    [Fact]
    public async void CreateAsync_should_return_Frederik()
    {
        //given
        CommitCreateDTO commit = new CommitCreateDTO("Arepo", DateTime.Parse("10/10-2022"), "amessage", "Frederik", "arepo");
        var (status, dto) = await _repo.CreateAsync(commit);

        status.Should().Be(Status.Created);
        dto.AuthorName.Should().Be("Frederik");
    }

    public async void CreateAsync_should_return_conflict_and_null()
    {
        //given
        CommitCreateDTO commit = new CommitCreateDTO("Arepo", DateTime.Parse("10/10-2022"), "Yoyoyo", "Frederik", "arepo");
        var (status, dto) = await _repo.CreateAsync(commit);

        status.Should().Be(Status.Conflict);
        dto.AuthorName.Should().Be(null);
    }

    [Fact]
    public async void FindAsync_should_return_Frederik()
    {
        var (status, dto) = await _repo.FindAsync(1);
        status.Should().Be(Status.OK);
        dto.AuthorName.Should().Be("Frederik");

    }

    [Fact]
    public async void FindAsync_should_return_notfound_and_null()
    {
        var (status, dto) = await _repo.FindAsync(2);
        status.Should().Be(Status.NotFound);
        dto.Should().Be(null);
    }

    [Fact]
    public async void ReadAsync_should_return_count_1()
    {
        var result = await _repo.ReadAsync();
        result.Count.Should().Be(1);
        var dto = result.First();
        dto.Id.Should().Be(1);
        dto.Message.Should().Be("Yoyoyo");
    }
}
