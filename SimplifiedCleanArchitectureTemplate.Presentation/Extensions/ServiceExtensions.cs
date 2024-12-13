namespace SimplifiedCleanArchitectureTemplate.Presentation.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    // Here is where we do our dependency setup
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(typeof(IDbBase<>), typeof(InMemDatabase<>));
        services.AddSingleton(typeof(IDbBaseService<>), typeof(DbBaseService<>));
    }
}