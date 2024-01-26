using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configurations
{
    public class AddTimezoneHeaderParameters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "time-zone",
                Required = false,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(TimeZoneInfo.Local.StandardName),
                    Enum = TimeZoneInfo.GetSystemTimeZones().Select(x => new OpenApiString(x.StandardName)).ToArray()
                },
            });
        }
    }

}
