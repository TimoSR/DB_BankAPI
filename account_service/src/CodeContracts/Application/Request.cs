using System.Text.Json.Serialization;

namespace CodeContracts.Application;

public class Request : IRequest
{
    [JsonIgnore]
    public Guid Id { get; init; } = Guid.NewGuid();
}