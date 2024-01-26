using API.DI.ConfigurationOptions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Configurations
{
    public class AddCultureHeaderParameters : IOperationFilter
    {
        private readonly SupportedCultureOptions _cultureOptionValue;

        public AddCultureHeaderParameters(IOptions<SupportedCultureOptions> cultureOptions)
        {
            _cultureOptionValue = cultureOptions.Value;
        }
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "culture",
                Required = false,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("en-US"),
                    Enum = _cultureOptionValue.Cultures.Select(x => new OpenApiString(x)).ToArray()
                },
            });
        }
    }

}
