using FormsApi.Contract.PropertyMetadata;

namespace FormsApi.Metadata.Builders;

public interface IMetadataBuilder<TModel>
{
    ControlType GetControlType();
}
