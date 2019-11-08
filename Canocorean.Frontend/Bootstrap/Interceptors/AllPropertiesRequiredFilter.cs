using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Canocorean.Frontend.Bootstrap.Interceptors
{
    /// <summary>
    /// Use this filter to prevent generation of typescript optional fields on frontend since it's totally incorrect implementation of backend nullable properties
    /// </summary>
    public class AllPropertiesRequiredFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                schema.Required = schema.Properties.Keys.ToList();
            }
        }
    }
}