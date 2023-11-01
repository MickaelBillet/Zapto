using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WeatherZapto.WebServer.Helpers
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        private string _tenantIdExample;

        public AddRequiredHeaderParameter(string tenantIdExample)
        {
            if (string.IsNullOrEmpty(tenantIdExample))
                throw new ArgumentNullException(nameof(tenantIdExample));

            _tenantIdExample = tenantIdExample;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "X-Forwarded-Path",
                Description = "Header",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema() { Type = "String" },
                Required = false,
                Example = new OpenApiString(_tenantIdExample)
            });
        }
    }
}
