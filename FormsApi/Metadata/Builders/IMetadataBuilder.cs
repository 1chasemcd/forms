using FormsApi.Contract.ControlMetadata;

namespace FormsApi.Metadata.Builders;

public interface IMetadataBuilder<TModel>
{
    ControlType GetControlType();
}
