using FormsApi.Definition.InputMetadata;

namespace FormsApi.Builder.Metadata;

public interface IMetadataBuilder<TModel>
{
    InputType GetInputType();
}
