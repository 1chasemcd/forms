using FormsApi.Builder;
using FormsApi.Definition.Primitives;

namespace FormsApi.Definition.Service;

public sealed class FormRegistry(MetadataBuilderService metadataBuilder)
{
    private readonly Dictionary<string, Form> _registry = [];

    internal void AddForm(string path, Form form)
    {
        if (!_registry.TryAdd(path, form))
            throw new InvalidOperationException($"Already had a registration for path '{path}'");
    }

    internal FormDto? TryGet(string path)
    {
        _registry.TryGetValue(path, out Form? form);
        if (form is null) return null;
        return new FormDto
        {
            View = form.GetView(),
            ModelType = new TypeDto(form.GetModelType()),
            ModelMetadatas = metadataBuilder.BuildMetadata(form.GetModelType())
        };
    }
}
