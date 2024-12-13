namespace SimplifiedCleanArchitectureTemplate.Presentation.Extensions;

[ExcludeFromCodeCoverage]
public static class EndpointExtensions
{
    public static void UseEndpoints<TMarker>(this IEndpointRouteBuilder app)
    {
        IEnumerable<TypeInfo> endpointTypes = GetEndpointTypesFromAssemblyContaining(typeof(TMarker));

        foreach (TypeInfo endpointType in endpointTypes)
        {
            endpointType.GetMethod(nameof(IEndpoints.DefineEndpoints))!
                .Invoke(null, new object[] { app.NewVersionedApi(endpointType.Name) });
        }
    }

    private static IEnumerable<TypeInfo> GetEndpointTypesFromAssemblyContaining(Type typeMarker)
    {
        return typeMarker.Assembly.DefinedTypes
            .Where(x => !x.IsAbstract && !x.IsInterface &&
                        typeof(IEndpoints).IsAssignableFrom(x));
    }
}