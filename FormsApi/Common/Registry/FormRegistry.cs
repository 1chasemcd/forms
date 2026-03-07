using FormsApi.Form;

namespace FormsApi.Common.Registry;


public class FormRegistry
{
    protected readonly Dictionary<string, BaseForm> _registry = [];

    internal void AddForm(string path, BaseForm form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    internal BaseForm? TryGet(string path)
    {
        _registry.TryGetValue(path, out BaseForm? form);
        return form;
    }
}
