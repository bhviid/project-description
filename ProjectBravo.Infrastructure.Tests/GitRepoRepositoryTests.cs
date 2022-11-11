//using System.Reflection.Metadata;
//using Xunit.Sdk;

//namespace ProjectBravo.Infrastructure.Tests;

//public class GitRepoRepositoryTests
//{
//    private readonly IGitRepoRepository _repo;
//    private readonly GitContext _context;

//    public GitRepoRepositoryTests()
//    {
//        var connection = new SqliteConnection("Filename=:memory:");
//        connection.Open();

//        var options = new DbContextOptionsBuilder<GitContext>()
//            .UseSqlite(connection)
//            .Options;
        
//        var context = new GitContext(options);

//        _context = context;
//        _repo = new GitRepoRepository(_context);
//        _context.Database.EnsureCreated();
//    }

//    // some kind of conflict or update the existing?
//    [Fact]
//    public async Task CreateAsync_given_newer_but_already_existing_repo_updates_existing()
//    {
//        // Arrange
//        _context.Repos.Add(new GitRepository
//        {
//            Name = "Etrepo",
//            Authors = new HashSet<Author> { new Author { Name = "asger" } },
//            Commits = new HashSet<Commit> { new Commit { Message = "HVAS�", Date = DateTime.Today.AddDays(-10), Author = new Author { Name = "asger" } } },
//            LatestCommitDate = DateTime.Now,
//        });
//        _context.SaveChanges();

//        var toAdd = new GitRepositryCreateDTO("Etrepo2",
//            new List<string>() { "asger", "B�llebob" },
//            new List<CommitCreateDTO>()
//            {
//                new CommitCreateDTO(DateTime.Today, "This is the newest commit!","B�llebob", "Etrepo")
//            });

//        // Act
//        var gRepo = await _repo.CreateAsync(toAdd);

//        // Assert
//        gRepo.Authors.Should().BeEquivalentTo(new[] { "asger", "B�llebob" });
//        gRepo.CommitIds.Should().HaveCount(2);
//    }

//    [Fact]
//    public async Task Create_given_new_repo_returns_dto()
//    {
//        // Arrange
//        var toAdd = new GitRepositryCreateDTO("Etrepo",
//            new List<string>() { "Frederik" },
//            new List<CommitCreateDTO>());

//        // Act
//        var resultRepo = await _repo.CreateAsync(toAdd);

//        // Assert
//        resultRepo.Name.Should().Be("Etrepo");
        
//        resultRepo.Authors.Should().NotBeEmpty()
//            .And.HaveCount(1)
//            .And.ContainEquivalentOf(new[] { "Frederik" });
        
//        resultRepo.CommitIds.Should().BeEmpty();

//    }

//    [Fact]
//    public async Task Find_given_existing_repo_name_returns_repo()
//    {
//        // Arrange
//        _context.Repos.Add(new GitRepository
//        {
//            Name = "Etrepo",
//            Authors = new HashSet<Author> { new Author { Name = "Frederik" } },
//            Commits = new HashSet<Commit> { new Commit { Message = "HVAS�", Date = DateTime.Now, Author = new Author { Name = "asger" } } },
//            LatestCommitDate = DateTime.Now,
//        });
//        _context.SaveChanges();

//        // Act
//        var found = await _repo.FindAsync("Etrepo");

//        // Assert
//        found.Should().NotBeNull();
        
//        found!.Authors.Should().BeEquivalentTo(new[] { "Frederik" });
        
//        found.CommitIds.Should().NotBeEmpty()
//            .And.HaveCount(1);
//    }

//    [Fact]
//    public async Task Find_given_non_existing_repo_name_returns_null()
//    {
//        // Arrange

//        // Act
//        var found = await _repo.FindAsync("Etrepo");

//        // Assert
//        found.Should().BeNull();
//    }

//    [Fact]
//    public async Task Update_given_existing_succeeds()
//    {
//        // Arrange
//        _context.Repos.Add(new GitRepository
//        {
//            Name = "Etrepo",
//            Authors = new HashSet<Author> { new Author { Name = "Frederik" } },
//            Commits = new HashSet<Commit> { new Commit { Message = "HVAS�", Date = DateTime.Now.AddHours(-5), Author = new Author { Name = "asger" } } },
//            LatestCommitDate = DateTime.Now,
//        });
//        var newCommit = new Commit()
//        {
//            Message = "New commit", Date = DateTime.Now, Author = new Author("Billy")

//        };
//        _context.Commits.Add(newCommit);
//        _context.SaveChanges();


//        var toUpdateDto = new GitRepositryUpdateDTO(
//            "Etrepo", 
//            new List<string>(){ newCommit.Author.Name },
//            new List<int>(){ newCommit.Id });

//        // Act
//        var updated = await _repo.UpdateAsync(toUpdateDto);


//        // Assert
//        updated.CommitIds.Should().Contain(newCommit.Id);
//        updated.CommitIds.Should().HaveCount(2);
//    }

//    //[Fact] what should it do?
//    public async Task Update_given_not_existing_repo()
//    {
//        // Arrange
//        var toUpdateDto = new GitRepositryUpdateDTO(
//            "Etrepo",
//            new List<string>() { "Bob" },
//            new List<int>() { 1,2});

//        // Act

//        var hmm = await _repo.UpdateAsync(toUpdateDto);

//        // Assert
        
//    }
//}