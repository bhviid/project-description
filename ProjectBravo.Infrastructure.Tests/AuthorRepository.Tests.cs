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
	public async Task Create_author_async_returns_id_1_and_Asger()
	{
		// Given
		var author = new AuthorCreateDTO("Asger");
		
		// When
		var results = await _repo.CreateAsync(author);
		// Then
		results.Should().Be(new AuthorDTO(2, "Asger"));
		
	}

	[Fact]
	public async Task FindAsync_should_return_1_and_Frederik()
	{
		// Given
		var entitiyId = 1;
		// When
		var result = await _repo.FindAsync(entitiyId);
		// Then
		result.Name.Should().Be("Frederik");
		result.Id.Should().Be(1);
	}

	[Fact]
	public async Task Update_should_return_status_updated()
	{
		// Given
		var newAuthor = new AuthorDTO(1, "Nyt Navn");
	
		// When
		_repo.UpdateAsync(newAuthor);
        var result = await _repo.FindAsync(1);
		// Then
		result.Name.Should().Be("Nyt Navn");

    }

	[Fact]
	public async Task Read_async_should_return_name_frederik()
	{
		// Given
		var result = await _repo.ReadAsync();
		// When

		//Then
		result.FirstOrDefault().Name.Should().Be("Frederik");
	}

	[Fact]
	public async Task Delete_async_should_return_Deleted()
	{
		var result = await _repo.DeleteAsync(1);

		result.Should().Be(Status.Deleted);

	}

	
}






