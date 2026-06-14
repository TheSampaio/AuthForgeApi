using Application.Contracts;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace Presentation.Swagger
{
    public class SwaggerFilter : ISchemaFilter
    {
        public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema is not OpenApiSchema concreteSchema)
                return;

            if (context.Type == typeof(LoginRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "email": "mario.bros@email.com",
                    "password": ""
                }
                """);
            }
            else if (context.Type == typeof(RegisterRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "firstName": "Mario",
                    "lastName": "Bros",
                    "email": "mario.bros@email.com",
                    "password": "",
                    "birthdate": ""
                }
                """);
            }
            else if (context.Type == typeof(CreateApplicationRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "name": ""
                }
                """);
            }
            else if (context.Type == typeof(AssignUserRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "userId": "int",
                    "clientId": "guid",
                    "role": ""
                }
                """);
            }
            else if (context.Type == typeof(SsoLoginRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "email": "mario.bros@email.com",
                    "password": "",
                    "clientId": "guid"
                }
                """);
            }
            else if (context.Type == typeof(SsoRegisterRequest))
            {
                concreteSchema.Example = JsonNode.Parse("""
                {
                    "firstName": "Mario",
                    "lastName": "Bros",
                    "email": "mario.bros@email.com",
                    "password": "",
                    "birthdate": "",
                    "clientId": "guid"
                }
                """);
            }
        }
    }
}