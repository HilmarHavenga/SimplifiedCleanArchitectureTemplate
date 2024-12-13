namespace SimplifiedCleanArchitectureTemplate.Tests.Integration.V2;

[ExcludeFromCodeCoverage]
public class SimplifiedCleanArchitectureEndpointsV2Tests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
{
    private const string DEFAULT_MESSAGE = "Hello Integration Test";
    private readonly WebApplicationFactory<IApiMarker> _factory;
    private readonly HttpClient _httpClient;
    private const string _baseRoute = "/api/v2/greetings";
    private readonly List<string> _createdGreetingIds = new();

    public SimplifiedCleanArchitectureEndpointsV2Tests(WebApplicationFactory<IApiMarker> factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }


    [Fact]
    public async Task CreateGreetingV2_CreatesGreeting_WhenDataIsCorrect()
    {
        // Arrange
        Greeting greetingToCreate = CreateGreeting();
        _createdGreetingIds.Add(greetingToCreate.Id);


        // Act
        HttpResponseMessage result = await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        Greeting? createdGreeting = await result.Content.ReadFromJsonAsync<Greeting>();


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        createdGreeting.Should().BeEquivalentTo(greetingToCreate);
    }


    [Fact]
    public async Task CreateGreetingV2_CreatesGreeting_WhenGreetingAlreadyExists()
    {
        // Arrange
        Greeting greetingToCreate = CreateGreeting();
        _createdGreetingIds.Add(greetingToCreate.Id);


        // Act
        await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        HttpResponseMessage result = await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        string? responseMessage = await result.Content.ReadFromJsonAsync<string>();

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseMessage.Should().BeEquivalentTo("That greeting already exists. Try a different one, or use the update endpoint");
    }


    [Fact]
    public async Task GetGreetingV2_ReturnsGreeting_WhenGreetingExists()
    {
        // Arrange
        Greeting greetingToCreate = CreateGreeting();
        _createdGreetingIds.Add(greetingToCreate.Id);
        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        Greeting? createdGreeting = await createResponse.Content.ReadFromJsonAsync<Greeting>();


        // Act
        HttpResponseMessage result = await _httpClient.GetAsync($"{_baseRoute}/{createdGreeting?.Id ?? "INVALID"}");
        Greeting? fetchedGreeting = await result.Content.ReadFromJsonAsync<Greeting>();


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        fetchedGreeting.Should().BeEquivalentTo(greetingToCreate);
    }


    [Fact]
    public async Task GetGreeting_ReturnsNotFound_WhenGreetingDoesNotExist()
    {
        // Arrange


        // Act
        HttpResponseMessage result = await _httpClient.GetAsync($"{_baseRoute}/-9999");


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task GetAllGreetingsV2_ReturnsAllGreetings_WhenGreetingExists()
    {
        // Arrange
        List<Greeting> greetingsToCreate = new();
        for (int i = 0; i < 4; i++)
        {
            Greeting greetingToCreate = CreateGreeting();
            greetingsToCreate.Add(greetingToCreate);
            _createdGreetingIds.Add(greetingToCreate.Id);
        }

        foreach (Greeting greetingToCreate in greetingsToCreate)
        {
            await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        }

        // Act
        HttpResponseMessage result = await _httpClient.GetAsync(_baseRoute);
        List<Greeting>? fetchedGreetings = await result.Content.ReadFromJsonAsync<List<Greeting>>();


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        fetchedGreetings.Should().BeEquivalentTo(greetingsToCreate);
    }


    [Fact]
    public async Task GetAllGreetingsV2_ReturnsNoGreetings_WhenNoGreetingsExist()
    {
        // Arrange


        // Act
        HttpResponseMessage result = await _httpClient.GetAsync(_baseRoute);
        List<Greeting>? returnedGreeting = await result.Content.ReadFromJsonAsync<List<Greeting>>();


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        returnedGreeting.Should().BeEmpty();
    }


    [Fact]
    public async Task UpdateGreetingV2_UpdatesdGreeting_WhenGreetingExists()
    {
        // Arrange
        Greeting greetingToCreate = CreateGreeting();
        _createdGreetingIds.Add(greetingToCreate.Id);
        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync(_baseRoute, greetingToCreate);
        Greeting? createdGreeting = await createResponse.Content.ReadFromJsonAsync<Greeting>();
        Greeting greetingToUpdate = new()
        {
            Message = $"Updated {DEFAULT_MESSAGE}",
            Id = createdGreeting?.Id ?? greetingToCreate.Id
        };


        // Act
        HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"{_baseRoute}/{greetingToUpdate.Id}", greetingToUpdate);
        Greeting? updatedGreeting = await result.Content.ReadFromJsonAsync<Greeting>();


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        updatedGreeting.Should().BeEquivalentTo(greetingToUpdate);
    }


    [Fact]
    public async Task UpdateGreeting_ReturnsNotFound_WhenGreetingDoesNotExist()
    {
        // Arrange
        Greeting greetingToUpdate = new()
        {
            Message = $"Updated {DEFAULT_MESSAGE}"
        };

        // Act
        HttpResponseMessage result = await _httpClient.PutAsJsonAsync($"{_baseRoute}/-9999", greetingToUpdate);


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact]
    public async Task DeleteGreetingV2_ReturnsNoContent_WhenGreetingHasBeenDeleted()
    {
        // Arrange
        Greeting greeting = CreateGreeting();
        _createdGreetingIds.Add(greeting.Id);
        HttpResponseMessage createResponse = await _httpClient.PostAsJsonAsync(_baseRoute, greeting);
        Greeting? createdGreeting = await createResponse.Content.ReadFromJsonAsync<Greeting>();


        // Act
        HttpResponseMessage result = await _httpClient.DeleteAsync($"{_baseRoute}/{createdGreeting?.Id ?? greeting.Id}");


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }


    [Fact]
    public async Task DeleteGreeting_ReturnsNotFound_WhenGreetingDoesNotExist()
    {
        // Arrange


        // Act
        HttpResponseMessage result = await _httpClient.DeleteAsync($"{_baseRoute}/-9999");


        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    private static Greeting CreateGreeting(string message = DEFAULT_MESSAGE)
    {
        return new Greeting
        {
            Message = message
        };
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    //We can implement this because we are using async methods. This comes from the IAsyncLifetime interface
    //Have to run this because integration tests write to the database
    //Better way is to spin up a whole new database in docker
    public async Task DisposeAsync()
    {
        HttpClient httpClient = _factory.CreateClient();

        foreach (string createdId in _createdGreetingIds)
        {
            await httpClient.DeleteAsync($"{_baseRoute}/{createdId}");
        }
    }
}