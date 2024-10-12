using System.Text.Json.Serialization;

namespace DemoIII;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(int))]
internal partial class HeartRateJsContext : JsonSerializerContext
{
    // This type's attributes specify JSON serialization info to preserve type structure
    // for trimmed builds.
}