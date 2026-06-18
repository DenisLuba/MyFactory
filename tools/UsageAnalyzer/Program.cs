using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var solutionRoot = args.Length > 0 ? args[0] : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
var viewModelsRoot = Path.Combine(solutionRoot, "src", "MyFactory.MauiClient", "ViewModels");
var servicesRoot = Path.Combine(solutionRoot, "src", "MyFactory.MauiClient", "Services");

if (!Directory.Exists(viewModelsRoot) || !Directory.Exists(servicesRoot))
{
    Console.Error.WriteLine("Could not locate ViewModels or Services directories.");
    return 1;
}

var viewModelFiles = Directory.GetFiles(viewModelsRoot, "*ViewModel.cs", SearchOption.AllDirectories);
var serviceFiles = Directory.GetFiles(servicesRoot, "*Service.cs", SearchOption.AllDirectories);

var viewModelInfos = new List<ViewModelInfo>();
var usedServiceMethods = new HashSet<ServiceMethodSignature>(ServiceMethodSignature.Comparer);

foreach (var file in viewModelFiles)
{
    var text = File.ReadAllText(file);
    var tree = CSharpSyntaxTree.ParseText(text);
    var root = tree.GetCompilationUnitRoot();

    foreach (var classDecl in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
    {
        if (!classDecl.Identifier.Text.EndsWith("ViewModel", StringComparison.Ordinal))
        {
            continue;
        }

        var classInfo = new ViewModelInfo(classDecl.Identifier.Text, file);
        var serviceFields = ExtractServiceMembers(classDecl);

        foreach (var methodDecl in classDecl.Members.OfType<MethodDeclarationSyntax>())
        {
            var methodInfo = new MethodInfo(methodDecl.Identifier.Text);
            var invocations = methodDecl.DescendantNodes().OfType<InvocationExpressionSyntax>();
            foreach (var invocation in invocations)
            {
                var access = invocation.Expression;
                var serviceField = GetServiceFieldName(access);
                if (serviceField is null)
                {
                    continue;
                }

                if (!serviceFields.TryGetValue(serviceField, out var serviceType))
                {
                    continue;
                }

                var methodName = ExtractMemberName(access);
                if (methodName is null)
                {
                    continue;
                }

                var signature = new ServiceMethodSignature(serviceType, methodName);
                usedServiceMethods.Add(signature);
                methodInfo.ServiceCalls.Add(signature);
            }

            classInfo.Methods.Add(methodInfo);
        }

        viewModelInfos.Add(classInfo);
    }
}

var serviceInfos = new List<ServiceInfo>();
foreach (var file in serviceFiles)
{
    var text = File.ReadAllText(file);
    var tree = CSharpSyntaxTree.ParseText(text);
    var root = tree.GetCompilationUnitRoot();
    foreach (var classDecl in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
    {
        if (!classDecl.Identifier.Text.EndsWith("Service", StringComparison.Ordinal))
        {
            continue;
        }

        var interfaceNames = classDecl.BaseList?.Types
            .Select(t => t.Type.ToString())
            .Where(name => name.Contains("Service", StringComparison.Ordinal))
            .ToImmutableArray() ?? ImmutableArray<string>.Empty;

        var classInfo = new ServiceInfo(classDecl.Identifier.Text, interfaceNames.ToArray(), file);
        foreach (var methodDecl in classDecl.Members.OfType<MethodDeclarationSyntax>())
        {
            if (!methodDecl.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
            {
                continue;
            }

            var methodName = methodDecl.Identifier.Text;
            var isUsed = interfaceNames.Any(name => usedServiceMethods.Contains(new ServiceMethodSignature(name, methodName)));
            classInfo.Methods.Add(new ServiceMethodInfo(methodName, isUsed));
        }

        serviceInfos.Add(classInfo);
    }
}

var output = new AnalysisResult(viewModelInfos, serviceInfos);
var jsonPath = Path.Combine(solutionRoot, "service-usage-analysis.json");
var json = System.Text.Json.JsonSerializer.Serialize(output, new System.Text.Json.JsonSerializerOptions
{
    WriteIndented = true
});
File.WriteAllText(jsonPath, json);
Console.WriteLine($"Analysis written to {jsonPath}");
return 0;

static Dictionary<string, string> ExtractServiceMembers(ClassDeclarationSyntax classDecl)
{
    var map = new Dictionary<string, string>(StringComparer.Ordinal);
    var fieldDecls = classDecl.Members.OfType<FieldDeclarationSyntax>();
    foreach (var field in fieldDecls)
    {
        var typeName = field.Declaration.Type.ToString();
        if (!typeName.Contains("Service", StringComparison.Ordinal))
        {
            continue;
        }

        foreach (var variable in field.Declaration.Variables)
        {
            map[variable.Identifier.Text] = typeName;
        }
    }

    var propertyDecls = classDecl.Members.OfType<PropertyDeclarationSyntax>();
    foreach (var property in propertyDecls)
    {
        var typeName = property.Type.ToString();
        if (!typeName.Contains("Service", StringComparison.Ordinal))
        {
            continue;
        }

        map[property.Identifier.Text] = typeName;
    }

    return map;
}

static string? GetServiceFieldName(ExpressionSyntax expression)
{
    switch (expression)
    {
        case MemberAccessExpressionSyntax memberAccess:
            return ExtractIdentifier(memberAccess.Expression);
        case IdentifierNameSyntax identifier:
            return identifier.Identifier.Text;
        case MemberBindingExpressionSyntax memberBinding when memberBinding.Parent is ConditionalAccessExpressionSyntax conditional:
            return ExtractIdentifier(conditional.Expression);
        default:
            return null;
    }
}

static string? ExtractMemberName(ExpressionSyntax expression)
    => expression switch
    {
        MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.Text,
        IdentifierNameSyntax identifier => identifier.Identifier.Text,
        MemberBindingExpressionSyntax memberBinding => memberBinding.Name.Identifier.Text,
        _ => null
    };

static string? ExtractIdentifier(ExpressionSyntax expression)
{
    return expression switch
    {
        IdentifierNameSyntax identifier => identifier.Identifier.Text,
        MemberAccessExpressionSyntax memberAccess => memberAccess.Name.Identifier.Text,
        _ => null
    };
}

record ServiceMethodSignature(string ServiceType, string MethodName)
{
    public static IEqualityComparer<ServiceMethodSignature> Comparer { get; } = new SignatureComparer();

    private sealed class SignatureComparer : IEqualityComparer<ServiceMethodSignature>
    {
        public bool Equals(ServiceMethodSignature? x, ServiceMethodSignature? y)
            => x is not null && y is not null &&
               string.Equals(x.ServiceType, y.ServiceType, StringComparison.Ordinal) &&
               string.Equals(x.MethodName, y.MethodName, StringComparison.Ordinal);

        public int GetHashCode(ServiceMethodSignature obj)
            => HashCode.Combine(obj.ServiceType, obj.MethodName);
    }
}

record MethodInfo(string Name)
{
    public List<ServiceMethodSignature> ServiceCalls { get; } = new();
}

record ViewModelInfo(string Name, string FilePath)
{
    public List<MethodInfo> Methods { get; } = new();
}

record ServiceMethodInfo(string Name, bool IsUsed);

record ServiceInfo(string Name, string[] Interfaces, string FilePath)
{
    public List<ServiceMethodInfo> Methods { get; } = new();
}

record AnalysisResult(List<ViewModelInfo> ViewModels, List<ServiceInfo> Services);
