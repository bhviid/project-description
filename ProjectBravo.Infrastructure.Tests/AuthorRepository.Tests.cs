using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.HttpResults;
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
        _context.Database.EnsureCreatedAsync();
        _repo = new AuthorsRepository(_context, new AuthorValidator());
		_context.Add(new AuthorEntity("Frederik", "frederik@gmail.com"));
		_context.SaveChanges();
	}
	[Fact]
	public async Task Create_author_async_returns_id_1_and_Asger()
	{
		// Given
		var author = new Author { Name = "Asger", Email= "asger@gmail.com" };
		
		// When
		var results = await _repo.CreateAsync(author);
		var created = results.Result as Created<Author>;
		// Then

		created!.Value.Should().Be(new Author { Id = 2, Name = "Asger", Email = "asger@gmail.com" });
		
		
	}

	[Fact]
	public async Task FindAsync_should_return_1_and_Frederik()
	{
		// Given
		var entitiyId = 1;
		// When
		var result = await _repo.FindAsync(entitiyId);
		// Then
		var found = result.Result as Ok<Author>;
		found!.Value.Should().Be(new Author { Id = 1, Name = "Frederik", Email = "frederik@gmail.com" });
		
	}

	[Fact]
	public async Task Update_should_return_status_updated()
	{
		// Given
		var author = new Author {  Name = "New name", Email = "new@mail.com" };
		

		// When
		var result = await _repo.UpdateAsync(1, author);
        result.Result.Should().BeOfType<NoContent>();
        var found = await _repo.FindAsync(1);

        // Then
        var updated = found.Result as Ok<Author>;
        updated!.Value.Should().Be(new Author { Id = 1, Name = "New name", Email = "new@mail.com" });


    }

	[Fact]
	public async Task Read_async_should_return_name_frederik()
	{
		// Given
		var result = await _repo.ReadAsync();
		// When

		//Then
		result.FirstOrDefault()!.Name.Should().Be("Frederik");
	}

	[Fact]
	public async Task Delete_async_should_return_Deleted()
	{
		var result = await _repo.DeleteAsync(1);
		var deleted = result.Result;
		deleted.Should().BeOfType<NoContent>();

	}


}






