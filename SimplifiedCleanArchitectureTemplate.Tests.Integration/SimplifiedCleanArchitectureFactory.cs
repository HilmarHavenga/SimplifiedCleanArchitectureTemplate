namespace SimplifiedCleanArchitectureTemplate.Tests.Integration;

[ExcludeFromCodeCoverage]
public class SimplifiedCleanArchitectureFactory : WebApplicationFactory<IApiMarker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //Here you can override the dependency injection
        base.ConfigureWebHost(builder);
    }
}