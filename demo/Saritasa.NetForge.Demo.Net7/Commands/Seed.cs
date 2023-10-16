using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using Saritasa.NetForge.Demo.Net7.Infrastructure.Seeders;

namespace Saritasa.NetForge.Demo.Net7.Commands;

/// <summary>
/// Seed command. It is used to run seeders in Infrastructure.Seeders namespace. It calls
/// the Seed method of seeder with arguments that user passed thru command line. Required arguments are
/// Name (class name) and Count.
/// </summary>
[HelpOption]
[Command("seed", Description = "Seed database with fake data.",
    UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
public class Seed
{
    private const string SeederNamespace = "Saritasa.NetForge.Demo.Net7.Infrastructure.Seeders";
    private const string SeedMethodName = "Seed";

    private readonly ILogger<Seed> logger;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// The name (class) of the seeder.
    /// </summary>
    [Option("--name", Description = "Name of seeder.")]
    [Required]
    public required string Name { get; init; }

    /// <summary>
    /// Number of objects to generate.
    /// </summary>
    [Option("--count", Description = "Number of objects to generate.")]
    public int Count { get; } = 200;

    /// <summary>
    /// Remaining not parsed arguments.
    /// </summary>
    public string[]? RemainingArguments { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Seed" /> class.
    /// </summary>
    /// <param name="logger">Logger.</param>
    /// <param name="serviceProvider">Service provider.</param>
    public Seed(ILogger<Seed> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Command line application execution callback.
    /// </summary>
    public async Task OnExecuteAsync()
    {
        if (Count < 1)
        {
            logger.LogInformation("Nothing to generate.");
            return;
        }

        // Run default seeder if no specific.
        var classType = !string.IsNullOrEmpty(Name) 
            ? Type.GetType($"{SeederNamespace}.{Name}Seeder", false, true) 
            : typeof(AddressesSeeder);

        if (classType == null)
        {
            logger.LogWarning("Class with name \"{Name}\" has not been found in the \"{SeederNamespace}\" namespace.",
                Name, SeederNamespace);
            return;
        }

        var seeder = CreateSeederFromType(classType);
        if (seeder == null)
        {
            logger.LogError("Cannot instantiate seeder {name}.", classType.Name);
            return;
        }

        try
        {
            await CallMethodWithRemainingArgumentsAsync(seeder, SeedMethodName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Cannot seed the data.");
            throw;
        }
    }

    private object? CreateSeederFromType(Type type)
    {
        // Find most descriptive ctor.
        var ctor = type
            .GetTypeInfo()
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
            .MaxBy(x => x.GetParameters().Length);
        
        if (ctor == null)
        {
            logger.LogError("Cannot find any public constructor for {name} type.", type.Name);
            return null;
        }

        var ctorParams = ctor.GetParameters();
        var inputParamObjects = new List<object?>(ctorParams.Length);
        inputParamObjects.AddRange(ctorParams.Select(paramInfo => serviceProvider
            .GetService(paramInfo.ParameterType)));

        return ctor.Invoke(inputParamObjects.ToArray());
    }

    /// <summary>
    /// Parse arguments like "--argName=10" to dictionary "argname":10.
    /// </summary>
    private IDictionary<string, string> ParseRemainingArguments()
    {
        var dict = new Dictionary<string, string>();
        
        if (RemainingArguments == null)
        {
            return dict;
        }
        
        foreach (var remainingArgument in RemainingArguments)
        {
            var args = remainingArgument.TrimStart('-').Split(new[] { '=', ':' }, 
                StringSplitOptions.RemoveEmptyEntries);
            
            if (args.Length != 2)
            {
                continue;
            }
            
            var argumentName = args[0].ToLower();
            var argumentValue = args[1];
            dict[argumentName] = argumentValue;
        }
        
        return dict;
    }

    private async Task CallMethodWithRemainingArgumentsAsync(object obj, string methodName)
    {
        // Resolve the method.
        var objType = obj.GetType();
        var seedMethod = objType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
        
        if (seedMethod == null)
        {
            logger.LogError("Class {objectName} must contain Seed method.", objType.Name);
            return;
        }

        // Collect and resolve input arguments.
        var args = ParseRemainingArguments();
        var inputParams = new List<object?>();
        
        foreach (var parameterInfo in seedMethod.GetParameters())
        {
            var value = parameterInfo.HasDefaultValue ? parameterInfo.DefaultValue : null;
            var nameKey = (parameterInfo.Name ?? string.Empty).ToLower();
            
            if (nameKey.Equals("numberOfItems", StringComparison.OrdinalIgnoreCase))
            {
                value = Count;
            }
            
            if (!string.IsNullOrEmpty(nameKey) && args.TryGetValue(nameKey, out var arg))
            {
                var tc = TypeDescriptor.GetConverter(parameterInfo.ParameterType);
                value = tc.ConvertFrom(arg);
            }
            inputParams.Add(value);
        }

        // Calling.
        var result = seedMethod.Invoke(obj, inputParams.ToArray());
        
        if (result is Task taskResult)
        {
            await taskResult;
        }
    }
}