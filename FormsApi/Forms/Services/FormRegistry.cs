using FormsApi.Contract.View;

namespace FormsApi.Forms.Services;

public interface IFormRegistry
{
    public void AddForm(string path, IForm form);
    Tuple<Type, BaseViewDto>? TryGet(string path);
}

internal sealed class FormRegistry() : IFormRegistry
{
    private readonly Dictionary<string, IForm> _registry = [];

    public void AddForm(string path, IForm form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    public Tuple<Type, BaseViewDto>? TryGet(string path)
    {
        _registry.TryGetValue(path, out IForm? form);
        if (form is null) return null;
        return new Tuple<Type, BaseViewDto>(form.GetModelType(), form.GetView());
    }
}
