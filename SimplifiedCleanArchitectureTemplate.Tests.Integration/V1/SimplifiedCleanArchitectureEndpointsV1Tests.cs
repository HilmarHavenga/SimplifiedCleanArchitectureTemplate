namespace SimplifiedCleanArchitectureTemplate.Tests.Integration.V1;

[ExcludeFromCodeCoverage]
public class SimplifiedCleanArchitectureEndpointsV1Tests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private const string _baseRoute = "/api/v1/greetings";

    public SimplifiedCleanArchitectureEndpointsV1Tests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetGreetingV1_GetsHelloWorld()
    {
        // Arrange
        HttpClient httpClient = _factory.CreateClient();

        // Act
        HttpResponseMessage result = await httpClient.GetAsync(_baseRoute);
        string foundGreeting = await result.Content.ReadAsStringAsync();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        foundGreeting.Should().BeEquivalentTo("Hello World");
    }
}