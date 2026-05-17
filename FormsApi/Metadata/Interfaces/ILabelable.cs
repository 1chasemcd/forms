using FormsApi.Common;

namespace FormsApi.Metadata.Interfaces;

public interface ILabelable<TModel>
{
    PropertyOrConstant<TModel, string?>? Label { get; set; }
}
public interface ILabelable<TThis, TModel> : ILabelable<TModel>;
