namespace SimplifiedCleanArchitectureTemplate.Presentation.Endpoints.V2;

public class GreetingEndpoints : IEndpoints
{
    private const string ContentType = "application/json";
    private const string Tag = "Greeting";
    private static readonly int _majorVersion = 2;
    private static readonly int _minorVersion = 0;

    public static void DefineEndpoints(IVersionedEndpointRouteBuilder app)
    {
        RouteGroupBuilder versioned = app.MapGroup("/api/v{version:apiVersion}/greetings").HasApiVersion(_majorVersion, _minorVersion);

        versioned.MapGet("/{id:int}", GetGreeting)
            .WithName("GetGreetingV2")
            .Produces(200)
            .Produces(404)
            .WithTags(Tag);

        versioned.MapGet("/", GetAllGreetings)
            .WithName("GetAllGreetingsV2")
            .Produces<IEnumerable<Greeting>>(200)
            .WithTags(Tag);

        versioned.MapPost("/", CreateGreeting)
            .WithName("CreateGreetingV2")
            .Accepts<Greeting>(ContentType)
            .Produces<Greeting>(201)
            .Produces(400)
            .WithTags(Tag);

        versioned.MapPut("/{id:int}", UpdateGreeting)
            .WithName("UpdateGreetingV2")
            .Accepts<Greeting>(ContentType)
            .Produces<Greeting>(200)
            .Produces(404)
            .WithTags(Tag);

        versioned.MapDelete("/{id:int}", DeleteGreeting)
            .WithName("DeleteGreetingV2")
            .Produces(204)
            .Produces(404)
            .WithTags(Tag);
    }

    internal static async Task<IResult> GetGreeting(string id, IDbBaseService<Greeting> greetingService)
    {
        IQueryable<KeyValuePair<string, Greeting>> foundGreetings = await greetingService.GetAsync(x => x.Key == id);

        if (!foundGreetings.Any())
        {
            return Results.NotFound("Could not find any greeting for that id");
        }

        return Results.Ok(foundGreetings.First().Value);
    }

    internal static async Task<IResult> GetAllGreetings(IDbBaseService<Greeting> greetingService)
    {
        List<KeyValuePair<string, Greeting>> foundGreetings = (await greetingService.GetAsync(x => true)).ToList();

        return Results.Ok(foundGreetings.Select(x => x.Value));
    }

    internal static async Task<IResult> CreateGreeting([FromBody] Greeting greeting, IDbBaseService<Greeting> greetingService)
    {
        bool created = await greetingService.CreateAsync(greeting);

        if (!created)
        {
            return Results.BadRequest("That greeting already exists. Try a different one, or use the update endpoint");
        }

        return Results.CreatedAtRoute("GetGreetingV2", new { id = greeting.Id }, greeting);
    }

    internal static async Task<IResult> UpdateGreeting(string id, [FromBody] Greeting greeting, IDbBaseService<Greeting> greetingService)
    {
        greeting.Id = id;
        bool updated = await greetingService.UpdateAsync(greeting);

        return updated ? Results.Ok(greeting) : Results.NotFound();
    }

    internal static async Task<IResult> DeleteGreeting(string id, IDbBaseService<Greeting> greetingService)
    {
        bool deleted = await greetingService.DeleteAsync(id);

        return deleted ? Results.NoContent() : Results.NotFound();
    }
}