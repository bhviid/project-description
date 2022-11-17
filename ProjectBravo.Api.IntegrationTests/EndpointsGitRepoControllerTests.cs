using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace ProjectBravo.Api.IntegrationTests;

//IClassFixture should tell Xunit to keep the same instance of the _factory for all tests.
public class EndpointsGitRepoControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public EndpointsGitRepoControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory(Skip = "Missing the persistancy - coming friday :DD")]
    [InlineData("/frequency/bhviid/project-description")]
    [InlineData("/author/bhviid/project-description")]
    public async Task Endpoints_returns_successStatusCode(string url)
    {
        // Arrange

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();

        // not sure what the response sohuld be tbh
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(Skip = "Missing the persistancy - coming friday :DD")]
    public async Task GetFrequency_endpoints_returns_succes()
    {
        // Arrange

        // Act
        var response = await _client.GetFromJsonAsync<string>("tjobi/dsys-chittychat");

        // Assert
        var nl = Environment.NewLine;

        response.TrimEnd()
            .Should()
            .Be($"1 2022-10-12{nl}2 2022-10-14{nl}1 2022-10-26{nl}1 2022-10-27{nl}1 2022-10-31");
    }
}