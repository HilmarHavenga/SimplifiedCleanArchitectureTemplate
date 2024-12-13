namespace SimplifiedCleanArchitectureTemplate.Presentation.Endpoints.V1;

public class GreetingEndpoints : IEndpoints
{
    private const string ContentType = "application/json";
    private const string Tag = "Greeting";
    private static readonly int _majorVersion = 1;
    private static readonly int _minorVersion = 0;

    public static void DefineEndpoints(IVersionedEndpointRouteBuilder app)
    {
        RouteGroupBuilder versioned = app.MapGroup("/api/v{version:apiVersion}/greetings").HasApiVersion(_majorVersion, _minorVersion);

        versioned.MapGet("/", () => "Hello World")
            .WithName("GetGreetingV1")
            .Produces(200)
            .Produces(404)
            .WithTags(Tag)
            .HasApiVersion(_majorVersion, _minorVersion);
    }
}