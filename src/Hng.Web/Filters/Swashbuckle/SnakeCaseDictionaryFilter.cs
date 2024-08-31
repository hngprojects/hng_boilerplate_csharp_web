using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hng.Web.Filters.Swashbuckle;

public class SnakeCaseDictionaryFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties.TryGetValue("placeholders", out OpenApiSchema value) && value.AdditionalProperties != null)
        {
            OpenApiSchema placeholdersSchema = value;
            OpenApiObject newExample = new()
            {
                ["key"] = new OpenApiString("value"),
                ["key2"] = new OpenApiString("value")
            };

            placeholdersSchema.Example = newExample;

            placeholdersSchema.AdditionalProperties = new OpenApiSchema { Type = "string" };

            // Ensure the changes are reflected in the main schema
            schema.Properties["placeholders"] = placeholdersSchema;
        }
    }
}
