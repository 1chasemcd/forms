using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IMaxLengthable<TModel>
{
    PropertyOrConstantBuilder<TModel, int>? MaxLength { get; set; }
}
public interface IMaxLengthable<TThis, TModel> : IMaxLengthable<TModel>;
