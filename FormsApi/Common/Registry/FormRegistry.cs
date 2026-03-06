using FormsApi.Form;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;


public class FormRegistry
{
    protected readonly Dictionary<string, FormDefinition> _registry = [];

    internal void AddForm(string path, FormDefinition form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    internal FormDefinition? TryGet(string path)
    {
        _registry.TryGetValue(path, out FormDefinition? form);
        return form;
    }
}
