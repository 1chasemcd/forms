using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface IMaxLengthable<TModel>
{
    PropertyOrConstant<TModel, int>? MaxLength { get; set; }
}
public interface IMaxLengthable<TThis, TModel> : IMaxLengthable<TModel>;
