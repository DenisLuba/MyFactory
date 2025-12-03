using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Auth;

var assembly = typeof(LoginRequest).Assembly;
var targetInterface = typeof(IExamplesProvider<>);
var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
	WriteIndented = true,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

var providerTypes = assembly
	.GetTypes()
	.Where(t => !t.IsAbstract && !t.IsInterface)
	.Select(t => new
	{
		Type = t,
		Interface = t.GetInterfaces()
			.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == targetInterface)
	})
	.Where(x => x.Interface is not null)
	.GroupBy(x => x.Type.Namespace?.Split('.')?.Last() ?? "Global")
	.OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase)
	.ToList();

var builder = new StringBuilder();

foreach (var group in providerTypes)
{
	builder.AppendLine($"### {group.Key}");
	builder.AppendLine();

	foreach (var entry in group.OrderBy(x => x.Type.Name, StringComparer.OrdinalIgnoreCase))
	{
		var provider = Activator.CreateInstance(entry.Type)
			?? throw new InvalidOperationException($"Cannot create instance of {entry.Type.FullName}");

		var method = entry.Type.GetMethod(nameof(IExamplesProvider<object>.GetExamples))
			?? throw new InvalidOperationException($"Cannot find GetExamples on {entry.Type.FullName}");

		var example = method.Invoke(provider, null);
		var exampleType = entry.Interface!.GetGenericArguments()[0];
		var json = JsonSerializer.Serialize(example, exampleType, jsonOptions);

		var displayName = entry.Type.Name.Replace("Example", string.Empty, StringComparison.OrdinalIgnoreCase);
		builder.AppendLine($"#### {displayName} ({exampleType.Name})");
		builder.AppendLine("```json");
		builder.AppendLine(json);
		builder.AppendLine("```");
		builder.AppendLine();
	}
}

var outputPath = args.Length > 0 ? args[0] : string.Empty;

if (string.IsNullOrWhiteSpace(outputPath))
{
	Console.Write(builder.ToString());
}
else
{
	var directory = Path.GetDirectoryName(outputPath);
	if (!string.IsNullOrEmpty(directory))
	{
		Directory.CreateDirectory(directory);
	}

	File.WriteAllText(outputPath, builder.ToString(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
}
