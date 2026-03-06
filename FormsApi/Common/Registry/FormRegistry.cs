using FormsApi.Form;
using Microsoft.Extensions.Logging;

namespace FormsApi.Common.Registry;


public class FormRegistry
{
    protected readonly Dictionary<string, FormModel> _registry = [];

    internal void AddForm(string path, FormModel form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    internal FormModel? TryGet(string path)
    {
        _registry.TryGetValue(path, out FormModel? form);
        return form;
    }
}
