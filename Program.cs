using MgtuLibrary.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

builder.Configuration.AddEnvironmentVariables(prefix: "DIGITAL_DIARY_");

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(options =>
{
	options.AddPolicy("CORSPolicy", b =>
	{
		b
			.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});


var connectionString = builder.Configuration.GetConnectionString("Default");
services.AddDbContext(connectionString);

services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


var app = builder.Build();

app.UseCors("CORSPolicy");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
	{
		var sb = new StringBuilder();
		foreach (var endpoint in endpointSources.SelectMany(es => es.Endpoints))
		{
			sb.Append(endpoint.DisplayName + ": ");

			if (endpoint is RouteEndpoint routeEndpoint)
			{
				sb.AppendLine(routeEndpoint.RoutePattern.RawText);
			}
		}
		return sb.ToString();
	});
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();