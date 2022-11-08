using System.Reflection.Metadata;
using Xunit;

namespace ProjectBravo.Infrastructure.Tests;

public class AuthorRepositoryTests
{
	private readonly IAuthorRepository _repo;
	private readonly GitContext _context;

	public AuthorRepositoryTests()
	{
		var connection = new SqliteConnection("Filename=:memory:");
		connection.Open();

		var options = new DbContextOptionsBuilder<GitContext>()
			.UseSqlite(connection)
			.Options;

		var context = new GitContext(options);

		_context = context;
		_repo = new AuthorsRepository(_context);
		_context.Database.EnsureCreatedAsync();
		_context.Add(new Author("Frederik"));
		_context.SaveChanges();
	}
	[Fact]
	public async void Create_author_async_returns_id_1_and_Asger()
	{
		// Given
		var author = new AuthorCreateDTO("Asger");
		
		// When
		var results = await _repo.CreateAsync(author);
		// Then
		results.Should().Be(new AuthorDTO(2, "Asger"));
		
	}

	[Fact]
	public void FindAsync_should_return_1_and_Frederik()
	{
		// Given
		var entitiyId = 1;
		// When
		var result = _repo.FindAsync(entitiyId);
		// Then
		result.Result.Name.Should().Be("Frederik");
		result.Result.Id.Should().Be(1);
	}

	[Fact]
	public void TestName()
	{
		// Given
	
		// When
	
		// Then
	}
	
	
}






