using System.Reflection;

namespace Utility;
public static class ResourceHelper
{
    public static string ReadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetCallingAssembly(); // Get the caller's assembly
        using Stream? stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new InvalidOperationException($"Resource {resourceName} not found.");
        }

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
