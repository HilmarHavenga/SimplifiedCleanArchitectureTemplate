namespace SimplifiedCleanArchitectureTemplate.Tests.Unit.Services;

[ExcludeFromCodeCoverage]
public class DbBaseServiceTests
{
    private readonly IDbBase<Greeting> _subDbService;

    public DbBaseServiceTests()
    {
        _subDbService = Substitute.For<IDbBase<Greeting>>();
    }


    [Fact]
    public async Task CreateGreeting_CreatesGreeting_WhenDataIsCorrect()
    {
        // Arrange
        IDbBaseService<Greeting> greetingService = new DbBaseService<Greeting>(_subDbService);
        Greeting greetingToCreate = CreateGreeting();

        _subDbService.CreateAsync(greetingToCreate).Returns(Task.FromResult(true));


        // Act
        bool result = await greetingService.CreateAsync(greetingToCreate);


        // Assert
        result.Should().BeTrue();
        await _subDbService.Received().CreateAsync(greetingToCreate);
    }


    [Fact]
    public async Task GetGreeting_ReturnsGreeting_WhenGreetingExists()
    {
        // Arrange
        IDbBaseService<Greeting> greetingService = new DbBaseService<Greeting>(_subDbService);

        // Need to create the predicate else assert and mock doesnt match
        Expression<Func<KeyValuePair<string, Greeting>, bool>> predicate = x => x.Key == "1";
        IQueryable<KeyValuePair<string, Greeting>> requiredResult = (new Dictionary<string, Greeting>()
        {
            { "", CreateGreeting() }
        }).AsQueryable();

        _subDbService.GetAsync(predicate).Returns(Task.FromResult(requiredResult));


        // Act
        IQueryable<KeyValuePair<string, Greeting>> result = await greetingService.GetAsync(predicate);


        // Assert
        result.Should().BeSameAs(requiredResult);
        await _subDbService.Received().GetAsync(predicate);
    }


    [Fact]
    public async Task UpdateGreeting_UpdatesGreeting_WhenDataIsCorrect()
    {
        // Arrange
        IDbBaseService<Greeting> greetingService = new DbBaseService<Greeting>(_subDbService);
        Greeting greetingToUpdate = CreateGreeting();

        _subDbService.UpdateAsync(greetingToUpdate).Returns(Task.FromResult(true));

        // Act
        bool result = await greetingService.UpdateAsync(greetingToUpdate);

        // Assert
        result.Should().BeTrue();
        await _subDbService.Received().UpdateAsync(greetingToUpdate);
    }


    [Fact]
    public async Task DeleteGreeting_DeletesGreeting_WhenGivenValidId()
    {
        // Arrange
        IDbBaseService<Greeting> greetingService = new DbBaseService<Greeting>(_subDbService);
        string greetingId = "1";

        _subDbService.DeleteAsync(greetingId).Returns(Task.FromResult(true));

        // Act
        bool result = await greetingService.DeleteAsync(greetingId);

        // Assert
        result.Should().BeTrue();
        await _subDbService.Received().DeleteAsync(greetingId);
    }

    private static Greeting CreateGreeting(string message = "Hello Unit Test")
    {
        return new Greeting
        {
            Id = "1",
            Message = message
        };
    }
}