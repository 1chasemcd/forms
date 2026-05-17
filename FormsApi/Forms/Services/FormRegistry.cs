using FormsApi.Contract.View;

namespace FormsApi.Forms.Services;

public sealed class FormRegistry()
{
    private readonly Dictionary<string, Form> _registry = [];

    internal void AddForm(string path, Form form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    internal Tuple<Type, BaseViewDto>? TryGet(string path)
    {
        _registry.TryGetValue(path, out Form? form);
        if (form is null) return null;
        return new Tuple<Type, BaseViewDto>(form.GetModelType(), form.GetView());
    }
}
