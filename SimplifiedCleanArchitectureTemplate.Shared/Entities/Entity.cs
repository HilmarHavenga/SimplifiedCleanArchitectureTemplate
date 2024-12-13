using Swashbuckle.AspNetCore.Annotations;

namespace SimplifiedCleanArchitectureTemplate.Shared.Entities;

public class Entity
{
    private static int _count = 1;

    //Simple implementation for now
    [SwaggerSchema(ReadOnly = true)]
    public string Id { get; set; } = (_count++).ToString();
}